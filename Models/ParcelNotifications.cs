using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class ParcelNotifications
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }
        public int DriverId { get; set; }
        public decimal DriverLat { get; set; }
        public decimal DriverLon { get; set; }
        public string DriverPlaceId { get; set; }
        public int RequestId { get; set; }
        public bool Accepted { get; set; }
        public bool IsNotificationSent { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime NotifySentDateTime { get; set; }
    }
}
