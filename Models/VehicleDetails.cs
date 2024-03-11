using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class VehicleDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleId { get; set; }
        [ForeignKey("VehicleTypes")]
        [Required]
        public int VehicleTypeId { get; set; }
        [ForeignKey("DriverDetails")]
        [Required]
        public int DriverId { get; set; }
        [ForeignKey("VehicleBrand")]
        [Required]
        public int BrandId { get; set; }
        [ForeignKey("VehicleColour")]
        [Required]
        public int VehicleColorId { get; set; }
        [Required]
        [MaxLength(50)]
        public string RegisterationNumber { get; set; }
        
        public VehicleTypes VehicleTypes { get; set; }
        public DriverDetails DriverDetails { get; set; }
        public VehicleBrand VehicleBrand { get; set; }
        public VehicleColour VehicleColour { get; set; }
    }
}
