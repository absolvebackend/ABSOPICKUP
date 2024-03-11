using System;

namespace _AbsoPickUp.ViewModels
{
    public class UserDetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DialCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPhoneNoVerified { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public int UserTypeId { get; set; }
        public string accessToken { get; set; }
        public string IDProof { get; set; }
        public string Selfie { get; set; }
        public int ScreenId { get; set; }
        public string LoginType { get; set; }
        public string ProfilePic { get; set; }
        public int ApplicationStatusId { get; set; }
        public string ApplicationStatus { get; set; }
        public int VehicleTypeId { get; set; }
        public string CountryCode { get; set; }
        public string AppStatusMessage { get; set; }
    }

    public class AdminDetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public int UserTypeId { get; set; }
        public string accessToken { get; set; }
        public string ProfilePic { get; set; }
    }

    public class UserListViewModel
    {
        public string UserId { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string DOB { get; set; }
        public string ApplicationStatus { get; set; }
        public DateTime JoiningDate { get; set; }
    }

    public class IndividualUserDetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DialCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int IDTypeId { get; set; }
        public string IDTypeValue { get; set; }
        public string IDProof { get; set; }
        public string IDCardBack { get; set; }
        public string UserId { get; set; }
        public string ProfilePic { get; set; }
        public int ApplicationStatus { get; set; }
        public bool IsPhoneNoVerified { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string DOB { get; set; }
        public string CreatedDate { get; set; }
    }

    public class BusinessUserDetailViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DialCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string ContactPerson { get; set; }
        public string LicenceNumber { get; set; }
        public string VAT { get; set; }
        public string ExternalNumber { get; set; }
        public string LicenceFilePath { get; set; }
        public string VATFilePath { get; set; }
        public string ChamberCommerceFilePath { get; set; }
        public string AgreementFilePath { get; set; }
        public int ApplicationStatus { get; set; }
        public bool IsPhoneNoVerified { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string CreatedDate { get; set; }

    }
    
    public class SuperAdminDefaultOptions
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
