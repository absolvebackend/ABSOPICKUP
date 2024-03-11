using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class DriverVehicleDocumentsViewModel
    {
        public IFormFile ProfilePic { get; set; }
        [MaxLength(15)]
        public string DOB { get; set; }
        public int DocTypeId { get; set; }
        public int DriverId { get; set; }
        public IFormFile PersonalId { get; set; }
        public IFormFile Selfie { get; set; }
        public IFormFile ResidentProof { get; set; }
    }

    public class DriverVehicleDocumentViewModel
    {
        public int DocTypeId { get; set; }
        public int DriverId { get; set; }
        public IFormFile DriverDocFile { get; set; }
    }

    public class UserDocumentsViewModel
    {
        public int DocTypeId { get; set; }
        public string UserId { get; set; }
        public int UserTypeId { get; set; }
        public IFormFile UserDocFile { get; set; }
    }
    public class BusinessUserDocumentsViewModel
    {
        public int DocTypeId { get; set; }
        public string UserId { get; set; }
        public int UserTypeId { get; set; }
        public string UserDocFile { get; set; }
    }
}
