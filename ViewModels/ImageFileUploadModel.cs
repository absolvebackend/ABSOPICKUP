using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class ImageFileUploadModel
    {
        [Required]
        public IFormFile ImgFile {get;set;}
    }
}
