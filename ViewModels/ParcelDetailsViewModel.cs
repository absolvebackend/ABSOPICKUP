using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class ParcelDetailsViewModel
    {
        public int RequestId { get; set; }
        public string ParcelName { get; set; }
        public string ParcelNotes { get; set; }
        [Required]
        public IFormFile ImgBeforePacking { get; set; }
        [Required]
        public IFormFile ImgAfterPacking { get; set; }
    }
}
