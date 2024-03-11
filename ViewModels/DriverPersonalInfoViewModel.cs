using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class DriverPersonalInfoViewModel
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(10)]
        public string DialCode { get; set; }
        [Required]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [MaxLength(128)]
        public string City { get; set; }
        public int ProvinceId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string DeviceType { get; set; }
        [Required]
        public string DeviceToken { get; set; }
    }
}
