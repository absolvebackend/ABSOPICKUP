using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class RegisterIndividualViewModel
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
        public int UserTypeId { get; set; }
        [Required]
        public string DeviceType { get; set; }
        [Required]
        public string DeviceToken { get; set; }
        [Required]
        public string DOB { get; set; }
        [Required]
        public int IDProofTypeId { get; set; }
        [Required]
        public IFormFile IDProof { get; set; }
        public IFormFile IDCardBack { get; set; }
    }

    public class UpdateIndividualViewModel
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
        public int UserTypeId { get; set; }
        [Required]
        public string DOB { get; set; }
        [Required]
        public int IDProofTypeId { get; set; }
        [Required]
        public IFormFile IDProof { get; set; }
        public IFormFile ProfilePic { get; set; }
        public IFormFile IDCardBack { get; set; }
    }

    public class UpdateIndividualResponseViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserTypeId { get; set; }
        public int IDProofTypeId { get; set; }
        public string IDProofImgPath { get; set; }
        public string ProfilePicImgPath { get; set; }
        public string IDCardBackImgPath { get; set; }
        public int ApplicationStatus { get; set; }
    }
}
