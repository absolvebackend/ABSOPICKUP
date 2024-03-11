using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class VehicleBrandViewModel
    {
        public int BrandId { get; set; }
        [Required]
        public string BrandName { get; set; }
        public int VehicleTypeId { get; set; }
    }
}
