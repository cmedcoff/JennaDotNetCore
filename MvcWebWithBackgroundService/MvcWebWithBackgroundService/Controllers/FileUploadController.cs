using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Azure.Storage.Blobs;

namespace MvcWebWithBackgroundService.Controllers
{
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

    public class LocalFileSystem : IFileStorageService
    {
        private readonly IOptions<FileStorageConfig> _config;

        public LocalFileSystem(IOptions<FileStorageConfig> config)
        {
            _config = config;
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
            CreateDirectoryIfNotExists(_config.Value.LocalFilePath);

            // might need some error handling
            var path = Path.Combine(_config.Value.LocalFilePath, file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }

    public class BlobStorage : IFileStorageService
    {
        private readonly IOptions<FileStorageConfig> _config;

        public BlobStorage(IOptions<FileStorageConfig> config)
        {
            _config = config;
        }

        public async Task UploadFileAsync(IFormFile file)
        {
            var blobServiceClient = new BlobServiceClient(_config.Value.BlobStorageConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_config.Value.BlobStorageContainerName);
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
}
