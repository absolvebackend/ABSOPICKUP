using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class VehicleColourViewModel
    {
        public int VehicleColorId { get; set; }
        [Required]
        public string VehicleColorName { get; set; }
        [Required]
        public string VehicleColorCode { get; set; }
    }
}
