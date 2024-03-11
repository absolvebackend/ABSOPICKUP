using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace _AbsoPickUp.Models
{
    public class DeliveryDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryId { get; set; }
        public int DriverId { get; set; }
        [ForeignKey("DeliveryRequest")]
        [Required]
        public int RequestId { get; set; }
        [ForeignKey("DeliveryStatus")]
        [Required]
        public int StatusId { get; set; }
        public decimal DriverLat { get; set; }
        public decimal DriverLong { get; set; }
        public DateTime DeliveryDateTime { get; set; }

        public DeliveryRequest DeliveryRequest { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
    }
}
