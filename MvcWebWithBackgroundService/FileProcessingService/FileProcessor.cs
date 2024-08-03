using Microsoft.Extensions.Options;
using Azure.Storage.Blobs;

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
    public bool UseBlobStorage { get; set; }
    public string? UploadPath { get; set; }
    public string? BlobStorageConnectionString { get; set; }
    public string? BlobStorageContainerName { get; set; }
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
        if (_config.UploadPath != null)
        {
            return Directory.GetFiles(_config.UploadPath);
        }
        else
        {
            // this really should be some custom exception type
            throw new Exception("UploadPath is not set in appsettings.json");
        }
    }
}

// in the long run this should be in a separate file, perhaps even a separate project
// per RCM Principles but for simplicity I'm keeping it here for now
ppublic class BlobStorage : IFileStorage
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
