namespace MvcWebWithBackgroundService.BackgroundServices;

public class FileProcesser : BackgroundService
{
    private readonly ILogger<FileProcesser> _logger;
    private readonly string? _uploadPath;

    public FileProcesser(ILogger<FileProcesser> logger)
    {
        _logger = logger;
        _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads");
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            CreateUploadDirIfNotExists();
            LogFilesToProcess();

            while (!stoppingToken.IsCancellationRequested)
            {
                LogFilesToProcess();
                await Task.Delay(5000, stoppingToken);
            }

        }
        catch (Exception)
        {
            throw;
        }

        return;
    }

    private void CreateUploadDirIfNotExists()
    {
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }

    }
    private void LogFilesToProcess()
    {
        var files = Directory.GetFiles(_uploadPath);
        _logger.LogInformation($"Found {files.Length} files to process.");

        foreach (var file in files)
        {
            _logger.LogInformation($"{file}.");
        }
    }

}
