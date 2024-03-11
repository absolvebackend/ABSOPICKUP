using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class ParcelDelivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PINId { get; set; }
        public int DriverId { get; set; }
        public int RequestId { get; set; }
        public int OrderId { get; set; }
        public string ReceiverPhoneNo { get; set; }
        public int DeliveryPIN { get; set; }
        public bool IsVerified { get; set; }
        public DateTime PINSentAt { get; set; }
        public DateTime VerifiedAt { get; set; }

    }
}
