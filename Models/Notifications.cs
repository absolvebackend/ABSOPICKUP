using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class Notifications
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public int ToDriverId { get; set; }

        [MaxLength(256)]
        [Required]
        public string ToUserId { get; set; }

        [Required]
        public int Type { get; set; }
        public int RequestId { get; set; }

        [Required]
        public string Text { get; set; }

        public bool IsRead { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
