using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class DriverWorkStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }
        public int DriverId { get; set; }
        public bool OnlineStatus { get; set; }
        public DateTime StatusDateTime { get; set; }
    }
}
