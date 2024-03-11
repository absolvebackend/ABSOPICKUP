using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class Orders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [Required]
        public int RequestId { get; set; }
        [Required]
        [MaxLength(512)]
        public string SenderId { get; set; }
        [Required]
        public int DriverId { get; set; }
        [Required]
        public int OrderStatus { get; set; }
        public DateTime AcceptedDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public bool IsDriverRated { get; set; }
    }
}
