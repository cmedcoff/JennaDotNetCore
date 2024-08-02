using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class FileUploadModel
{
    [Required]
    [Display(Name = "File")]
    public IFormFile File { get; set; }
}
