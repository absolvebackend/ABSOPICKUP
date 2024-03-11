using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _AbsoPickUp.ViewModels
{
    public class ResetUserModel
    {
        [Required]
        public string EmailPhone { get; set; }
        [Required]
        public string Otp { get; set; }
        [Required]
        public string newPassword { get; set; }
    }

    public class ChangeDriverPasswordModel
    {
        [Required]
        public int DriverId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
