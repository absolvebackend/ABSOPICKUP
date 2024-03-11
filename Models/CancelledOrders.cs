using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class CancelledOrders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderCancelId { get; set; }
        [Required]
        [MaxLength(512)]
        public string SenderId { get; set; }
        [Required]
        public int OrderId { get; set; }
        public string CancelReason { get; set; }
        public DateTime CancelledDateTime { get; set; }
    }
}
