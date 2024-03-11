using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class DeliveryRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestId { get; set; }
        [Required]
        public string SenderId { get; set; }
        public string SenderAddress { get; set; }
        [MaxLength(512)]
        public string SenderPlaceId { get; set; }
        public decimal SenderLat { get; set; }
        public decimal SenderLong { get; set; }
        [MaxLength(50)]
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        [MaxLength(256)]
        public string ReceiverEmail { get; set; }
        public int DialCode { get; set; }
        [MaxLength(15)]
        public string ReceiverMobileNumber { get; set; }
        public decimal ReceiverLat { get; set; }
        public decimal ReceiverLong { get; set; }
        [MaxLength(512)]
        public string ReceiverPlaceId { get; set; }
        [Required]
        [ForeignKey("DeliveryTypes")]
        public int DeliveryTypeId { get; set; }
        [MaxLength(50)]
        public string TotalDeliveryTime { get; set; }
        [MaxLength(50)]
        public string TotalDeliveryDistance { get; set; }
        public DateTime RequestDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public DeliveryTypes DeliveryTypes { get; set; }
    }
}
