using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class TransactionMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        public string OrderId { get; set; }
         
        [MaxLength(256)]
        public string UserId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public int TransactionType { get; set; }

        [Required]
        [MaxLength(200)]
        public int PaymentMode { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
