using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class VehicleColour
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleColorId { get; set; }
        [Required]
        [MaxLength(50)]
        public string VehicleColorName { get; set; }
        public string VehicleColorCode { get; set; }
    }
}
