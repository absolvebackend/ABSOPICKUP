using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class ParcelDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParcelId { get; set; }
        [Required]
        public int RequestId { get; set; }
        [MaxLength(250)]
        public string ParcelName { get; set; }
        public string ParcelNotes { get; set; }
        [MaxLength(500)]
        public string ImgBeforePacking { get; set; }
        [MaxLength(500)]
        public string ImgAfterPacking { get; set; }
        
    }
}
