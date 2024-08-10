using System.Diagnostics;
using Microsoft.Extensions.Configuration.Binder;

using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace FileProcessingService;

// this type is by by design coupled to it's client FileProcessor
// so I'm ok with it being in the same file, but it could be in a separate file
// but close to the client
public interface IFileStorage
{
    IEnumerable<string> GetFiles();
}

// I want to be able to switch between local file storage and blob storage
// so their configuration can be coupled together, but it might be better to separate them
// as per interface segregation principle
public class FileStorageConfig
{
    public string? LocalFilePath { get; set; }
    public bool UseBlobStorage { get; set; }
    public string? BlobStorageConnectionString { get; set; }
    public string? BlobStorageContainerName { get; set; }
}

public class FileStorageConfigValidator : IValidateOptions<FileStorageConfig>
{
    public ValidateOptionsResult Validate(string? name, FileStorageConfig options)
    {
        try
        {
            if (options.UseBlobStorage)
            {
                if (string.IsNullOrWhiteSpace(options.BlobStorageConnectionString))
                {
                    return ValidateOptionsResult.Fail("BlobStorageConnectionString is required when UseBlobStorage is true");
                }
                else
                {
                    Console.WriteLine($"BlobStorageConnectionString: {options.BlobStorageConnectionString}");
                }

                if (string.IsNullOrWhiteSpace(options.BlobStorageContainerName))
                {
                    return ValidateOptionsResult.Fail("BlobStorageContainerName is required when UseBlobStorage is true");
                }

                var storageAccountClient = new BlobServiceClient(options.BlobStorageConnectionString);
                return storageAccountClient.GetBlobContainerClient(options.BlobStorageContainerName).Exists()
                    ? ValidateOptionsResult.Success
                    : ValidateOptionsResult.Fail($"BlobStorageContainerName '{options.BlobStorageContainerName}' does not exist");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(options.LocalFilePath))
                {
                    return ValidateOptionsResult.Fail("LocalFilePath is required when UseBlobStorage is false");
                }
            }
        }
        catch(Exception ex)
        {
            return ValidateOptionsResult.Fail($"BlobStorageConnectionString is invalid: {ex.Message}");
        }
    
        return ValidateOptionsResult.Success;
    }
}


// in the long run this should be in a separate file, perhaps even a separate project
// per RCM Principles but for simplicity I'm keeping it here for now
public class LocalFileStorage : IFileStorage
{
    private readonly FileStorageConfig _config;

    public LocalFileStorage(IOptions<FileStorageConfig> config)
    {
        _config = config.Value;
    }

    public IEnumerable<string> GetFiles()
    {
        if (_config.LocalFilePath != null)
        {
            return Directory.GetFiles(_config.LocalFilePath);
        }
        else
        {
            // this really should be some custom exception type
            throw new Exception("LocalFilePath is not set in appsettings.json");
        }
    }
}

// in the long run this should be in a separate file, perhaps even a separate project
// per RCM Principles but for simplicity I'm keeping it here for now
public class BlobStorage : IFileStorage
{
    private readonly FileStorageConfig _config;

    public BlobStorage(IOptions<FileStorageConfig> config)
    {
        _config = config.Value;
    }

    public IEnumerable<string> GetFiles()
    {
        var containerClient = new BlobContainerClient(_config.BlobStorageConnectionString, _config.BlobStorageContainerName);
        return containerClient.GetBlobs().Select(b => b.Name);
    }
}


public class FileProcessor : BackgroundService
{
    private readonly ILogger<FileProcessor> _logger;

    // by using the interface we can switch between local file storage and blob storage
    // without changing the client, e.g. Open-Closed Principle, and we've also decoupled
    // the client from the implementation, including any particular cloud storage provider
    // it's the same technique as always, dependency inversion principle and the
    // adapter pattern
    private readonly IFileStorage _fileStorage;

    public FileProcessor(ILogger<FileProcessor> logger, IFileStorage fileStorage)
    {
        _logger = logger;
        _fileStorage = fileStorage;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                LogFilesToProcess();
            }
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void LogFilesToProcess()
    {
        var files = _fileStorage.GetFiles();
        foreach (var file in files)
        {
            _logger.LogInformation($"File to process: {file}");
        }
    }
}

public static class FileProcessorServicesExt
{
    public static IServiceCollection AddFileProcessorService(this IServiceCollection services, IConfiguration configuration)
    {
        // add service
        services.AddHostedService<FileProcessor>();

        // now for dependent config and services
        var fileStorageConfigSection = configuration.GetSection(nameof(FileStorageConfig));

        // fetch config section and if to use blob storage and add the configured service0
        services.Configure<FileStorageConfig>(fileStorageConfigSection);
        services.AddSingleton(typeof(IFileStorage),
            fileStorageConfigSection.GetValue<bool>("UseBlobStorage") ? typeof(BlobStorage) : typeof(LocalFileStorage));


        services.AddOptions<FileStorageConfig>()
            .Bind(fileStorageConfigSection)
            //.ValidateDataAnnotations() // if using DataAnnotations for validation
            .ValidateOnStart();

        // alternative to DataAnnotaitons, Register a custom validator
        services.AddSingleton<IValidateOptions<FileStorageConfig>, FileStorageConfigValidator>();

        // a: IHostEnvironment.EnvironmentName == "Development"
        if (configuration.GetValue<string>("Environment") == "Development")
        {
            // write GetDebugView to a file
            using (var writer = new StreamWriter("config_debug.txt"))
            {
                writer.WriteLine(((IConfigurationRoot)configuration).GetDebugView());
            }
        }

        return services;
    }
}

