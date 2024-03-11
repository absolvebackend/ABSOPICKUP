using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class DriverBankDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccId { get; set; }
        [Required]
        public int DriverId { get; set; }
        [Required]
        [MaxLength(512)]
        public string AccountName { get; set; }
        [Required]
        [MaxLength(50)]
        public string AccountNumber { get; set; }
        [MaxLength(50)]
        public string BranchCode { get; set; }
        [MaxLength(50)]
        public string SwiftCode { get; set; }
        [Required]
        [MaxLength(512)]
        public string Bank { get; set; }
        [MaxLength(512)]
        public string Branch { get; set; }
    }
}
