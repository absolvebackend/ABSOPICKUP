using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class UserDetails : IdentityUser
    {
        [ForeignKey("UserTypes")]
        public int UserTypeId { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
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
        [MaxLength(256)]
        public string GoogleId { get; set; }
        [MaxLength(256)]
        public string FacebookId { get; set; }
        public bool IsSocialUser { get; set; }
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
        public int ApplicationStatus { get; set; }
        public string RejectReason { get; set; }
        public string DOB { get; set; }
        public UserTypes UserTypes { get; set; }

    }
}
