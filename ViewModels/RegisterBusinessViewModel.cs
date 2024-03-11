using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class RegisterBusinessViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string DialCode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Website { get; set; }
        [Required]
        [MaxLength(50)]
        public string ContactPerson { get; set; }
        [Required]
        [MaxLength(50)]
        public string LicenceNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string VAT { get; set; }
        [Required]
        [MaxLength(25)]
        public string ExternalNumber { get; set; }
        [Required]
        public IFormFile LicenceFilePath { get; set; }
        [Required]
        public IFormFile VATFilePath { get; set; }
        [Required]
        public IFormFile ChamberCommerceFilePath { get; set; }
        [Required]
        public IFormFile AgreementFilePath { get; set; }
        [Required]
        public int UserTypeId { get; set; }
        [Required]
        public string DeviceType { get; set; }
        [Required]
        public string DeviceToken { get; set; }
    }
}
