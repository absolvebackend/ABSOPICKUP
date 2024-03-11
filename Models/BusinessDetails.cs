using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class BusinessDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string UserId { get; set; }
        [Required]
        public string Website { get; set; }
        [Required]
        [MaxLength(50)]
        public string ContactPerson { get; set; }
        [Required]
        [MaxLength(50)]
        public string LicenceNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string VAT { get; set; }
        [Required]
        [MaxLength(100)]
        public string ExternalContractNumber { get; set; }
    }
}
