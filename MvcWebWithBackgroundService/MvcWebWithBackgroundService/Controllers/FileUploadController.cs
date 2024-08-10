using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Azure.Storage.Blobs;

namespace MvcWebWithBackgroundService.Controllers;

public interface IFileStorageService
{
    Task UploadFileAsync(IFormFile file);
}

public class FileStorageConfig
{
    public string? LocalFilePath { get; set; }
    public bool UseBlobStorage { get; set; }
    public string? BlobStorageConnectionString { get; set; }
    public string? BlobStorageContainerName { get; set; }
}

internal class FileStorageConfigValidator : IValidateOptions<FileStorageConfig>
{
    public ValidateOptionsResult Validate(string? name, FileStorageConfig options)
    {
        return string.IsNullOrEmpty(options.LocalFilePath) && !options.UseBlobStorage ? 
            ValidateOptionsResult.Fail("LocalFilePath is required when UseBlobStorage is false") : 
            ValidateOptionsResult.Success;
    }
}


public class LocalFileSystem : IFileStorageService
{
    private readonly FileStorageConfig _config;

    public LocalFileSystem(IOptions<FileStorageConfig> config)
    {
        _config = config.Value;
    }

    private void CreateDirectoryIfNotExists(string path)
    {
        // might need some error handling
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public async Task UploadFileAsync(IFormFile file)
    {
        if (string.IsNullOrEmpty(_config.LocalFilePath))
        {
            // I'd probably use a custom exception type here
            throw new InvalidOperationException("LocalFilePath is not configured");
        }
        else
        {
            CreateDirectoryIfNotExists(_config.LocalFilePath);
        }
        

        // might need some error handling
        var path = Path.Combine(_config.LocalFilePath, file.FileName);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
    }
}

public class BlobStorage : IFileStorageService
{
    private readonly FileStorageConfig _config;

    public BlobStorage(IOptions<FileStorageConfig> config)
    {
        _config = config.Value;
    }

    public async Task UploadFileAsync(IFormFile file)
    {
        var blobContainerClient = new BlobContainerClient(_config.BlobStorageConnectionString, _config.BlobStorageContainerName);
        await blobContainerClient.UploadBlobAsync(file.FileName, file.OpenReadStream());
    }
}

public class FileUploadController : Controller
{
    private readonly IFileStorageService _fileStorageService;

    public FileUploadController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ViewBag.Message = "File not selected";
            return View("Index");
        }

        await _fileStorageService.UploadFileAsync(file);

        //await ProcessFile(path);

        ViewBag.Message = "File uploaded successfully";
        return View("Index");
    }

    // private async Task ProcessFile(string path)
    // {
    //     // simulate some slow long code to process the contents of the file
    //     await Task.Delay(20000);
    // }
}



public static class FileStorageServiceExtensions
{
    public static IServiceCollection AddFileStorageService(this IServiceCollection services, IConfiguration configuration)
    {
        var fileStoreageConfig = configuration.GetSection(nameof(FileStorageConfig));
        services.Configure<FileStorageConfig>(fileStoreageConfig);

        // register the service based on the configuration
        services.AddSingleton(typeof(IFileStorageService), fileStoreageConfig.GetValue<bool>("UseBlobStorage") ? 
            typeof(BlobStorage) : typeof(LocalFileSystem));

        // add configuration validation
        services.AddSingleton<IValidateOptions<FileStorageConfig>>(new FileStorageConfigValidator());
        services.AddOptionsWithValidateOnStart<FileStorageConfig>();

        return services;
    }
}


