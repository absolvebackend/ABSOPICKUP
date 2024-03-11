using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class DriverDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DriverId { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(15)]
        public string DOB { get; set; }
        [Required]
        [MaxLength(25)]
        public int? Otp { get; set; }
        [MaxLength(10)]
        public string DialCode { get; set; }
        [Required]
        public string DeviceToken { get; set; }
        [Required]
        public string DeviceType { get; set; }
        [MaxLength(500)]
        public string ProfilePic { get; set; }
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
        [ForeignKey("SouthAfricaProvinces")]
        public int ProvinceId { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public int ApplicationStatus { get; set; }
        [MaxLength(256)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(256)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(256)]
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public int ScreenId { get; set; }
        public string RejectReason { get; set; }
        public SouthAfricaProvinces SouthAfricaProvinces { get; set; }

    }
}
