using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace MvcWebWithBackgroundService.Controllers
{
    public class FileUploadController : Controller
    {
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

            // again here I'd use DIP to extract this to an implementation of some interface
            // which can be implemente as a local file system, Azure Blob Storage, etc.
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //await ProcessFile(path);

            ViewBag.Message = "File uploaded successfully";
            return View("Index");
        }

        private async Task ProcessFile(string path)
        {
            // simulate some slow long code to process the contents of the file
            await Task.Delay(20000);
        }
    }
}
