using Microsoft.Extensions.Options;

namespace FileProcessingService;

public class FileProcessConfig
{
    public string? UploadPath { get; set; }
}


public class FileProcessor : BackgroundService
{
    private readonly ILogger<FileProcessor> _logger;
    private readonly IOptions<FileProcessConfig> _config;

    public FileProcessor(IOptions<FileProcessConfig> config, ILogger<FileProcessor> logger)
    {
        _config = config;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        CreateUploadDirIfNotExists();
        LogFilesToProcess();

        while (!stoppingToken.IsCancellationRequested)
        {
            // could use azure blob storage api to retrieve file from blog storage
            // processsing of the file, parse, insert data into database
            // regardless of approach, it is probably better to hide any of these 
            // file operations behind an interface and inject the interface into the
            // FileProcessor class so that this class adhere to the SOLID principles
            // and can be easily tested or replaced with a different implementation, e.g.
            // the local file system, or blog storage or whatever
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                LogFilesToProcess();
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
    private void CreateUploadDirIfNotExists()
    {
        if (!Directory.Exists(_config.Value.UploadPath))
        {
            if (_config.Value.UploadPath != null)
                Directory.CreateDirectory(_config.Value.UploadPath);
            else
                // I'd probably throw a custom exception here, but for simplicity
                throw new Exception("UploadPath is not set in appsettings.json");
        }
    }
    private void LogFilesToProcess()
    {
        if (_config.Value.UploadPath != null)
        {
            string[] files = Directory.GetFiles(_config.Value.UploadPath);
            _logger.LogInformation($"Found {files.Length} files to process.");
            foreach (var file in files)
            {
                _logger.LogInformation($"{file}.");
            }
        }
        else
            // I'd probably throw a custom exception here, but for simplicity
            throw new Exception("UploadPath is not set in appsettings.json");
    }
}
