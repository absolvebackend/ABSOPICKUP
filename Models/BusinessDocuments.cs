using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class BusinessDocuments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string UserId { get; set; }
        [Required]
        public string LicenceFilePath { get; set; }
        [Required]
        public string VATFilePath { get; set; }
        [Required]
        public string ChamberCommerceFilePath { get; set; }
        [Required]
        public string AgreementFilePath { get; set; }
    }
}
