using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class VehicleDetailsViewModel
    {
        [Required]
        public int VehicleTypeId { get; set; }
        [Required]
        public int DriverId { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int VehicleColorId { get; set; }
        [Required]
        [MaxLength(50)]
        public string RegisterationNumber { get; set; }
        public IFormFile FrontSideImage { get; set; }
        public IFormFile BackSideImage { get; set; }
    }

    public class VehicleDetailsUpdateResponseViewModel
    {
        public int ApplicationStatus { get; set; }
        public string FrontSideImgPath { get; set; }
        public string BackSideImgPath { get; set; }
    }
}
