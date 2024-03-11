using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class PaymentDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }
        [Required]
        [MaxLength(250)]
        public string UserId { get; set; }
        [ForeignKey("Orders")]
        [Required]
        public int OrderId { get; set; }
        [MaxLength(5)]
        public string Currency { get; set; }
        [Required]
        public double Amount { get; set; }
        [ForeignKey("PaymentStatus")]
        public int PaymentStatusId { get; set; }
        public int TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Orders Orders { get; set; }
    }
}
