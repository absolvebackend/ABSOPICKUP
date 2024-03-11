using _AbsoPickUp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class DriverInfoViewModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleColor { get; set; }
        public string RegistrationNumber { get; set; }
        public string ProfilePic { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public string CreatedDate { get; set; }
        public int VehicleTypeId { get; set; }
        public double TodayEarnings { get; set; }
        public double Rating { get; set; }
        public string DriverSelfieImgPath { get; set; }
        public string DriverLicenseImgPath { get; set; }
        public string DriverProofOfResidenceImgPath { get; set; }
        public string VehicleRegisterationImgPath { get; set; }
        public string VehicleFrontSideImgPath { get; set; }
        public string VehicleBackSideImgPath { get; set; }
        public int VehicleBrandId { get; set; }
        public int VehicleColorId { get; set; }
        public int ProvinceId { get; set; }
    }

    public class UpdateDriverInfoViewModel
    {
        [Required]
        public int DriverId { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public int ProvinceId { get; set; }
        [Required]
        public int PersonalIDDocTypeId { get; set; }
        [Required]
        public IFormFile ProfilePic { get; set; }
        [Required]
        public IFormFile SelfieImg { get; set; }
        [Required]
        public IFormFile PersonalIDImg { get; set; }
        [Required]
        public IFormFile ProofOfResidenceImg { get; set; }
    }

    public class UpdateDriverResponseViewModel
    {
        public int DriverId { get; set; }
        public int PersonalIDDocTypeId { get; set; }
        public string ProfilePic { get; set; }
        public string SelfieImg { get; set; }
        public string PersonalIDImg { get; set; }
        public string ProofOfResidenceImg { get; set; }
        public int ApplicationStatus { get; set; }
    }

    public class DriverInfoUpdateViewModel
    {
        public int DriverId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int ProvinceId { get; set; }
        public string ProfilePic { get; set; }
    }

    public class DriverListViewModel
    {
        public int DriverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string DOB { get; set; }
        public string ApplicationStatus { get; set; }
        public DateTime JoiningDate { get; set; }
    }

    public class RequestDriverInfoViewModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleColor { get; set; }
        public string RegistrationNumber { get; set; }

        public RequestDriverInfoViewModel()
        {
            Name = String.Empty;
            PhoneNumber = String.Empty;
            VehicleBrand = String.Empty;
            VehicleColor = String.Empty;
            RegistrationNumber = String.Empty;
        }
    }
    public class DriverReviewViewModel
    {
        public string UserName { get; set; }
        public string CommentedAt { get; set; }
        public string Review { get; set; }
        public double Rating { get; set; }
    }
    public class UserReviewViewModel
    {
        public string UserId { get; set; }
        public int DriverId { get; set; }
        public int OrderId { get; set; }
        public string Review { get; set; }
        public double Rating { get; set; }
    }
    public class DriverRatingViewModel
    {
        public List<DriverReviewViewModel> ReviewList { get; set; }
        public int DriverId { get; set; }
        public double UserRating { get; set; }
    }
}
