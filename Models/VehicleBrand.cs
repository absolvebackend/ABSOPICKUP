using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class VehicleBrand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrandId { get; set; }
        [Required]
        [MaxLength(50)]
        public string BrandName { get; set; }
        
        public int VehicleTypeId { get; set; }
    }
}
