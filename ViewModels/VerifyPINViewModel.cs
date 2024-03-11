using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class VerifyPINViewModel
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int DriverId { get; set; }
        [Required]
        public string ReceiverPhoneNo { get; set; }
        [Required]
        public int DeliveryPIN { get; set; }
    }
}
