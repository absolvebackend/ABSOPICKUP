using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class LoginViewModel
    {
        public string EmailPhone { get; set; }
        public string Password { get; set; }
        public string DeviceType { get; set; }
        public string DeviceToken { get; set; }
    }
    public class PhoneModel
    {
        [Required]
        public string DialCode { get; set; }
        [Required]
        public string PhoneNo { get; set; }
    }
    public class VerifyPhoneModel
    {
        [Required]
        public string code { get; set; }
    }
    public class VerifyDriverPhoneModel
    {
        [Required]
        public string DialCode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string code { get; set; }
    }
    public class VerifyUserModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string otp { get; set; }
    }
    public class EmailModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class ForgotPassword
    {
        [Required]
        public string EmailPhone { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
