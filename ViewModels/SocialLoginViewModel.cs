using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class SocialLoginViewModel
    {
        [Required]
        public string LoginType { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        public int UserTypeId { get; set; }
        [Required]
        public string EmailPhone { get; set; }
        [Required]
        public string SocialId { get; set; }
        [Required]
        public string DeviceType { get; set; }
        [Required]
        public string DeviceToken { get; set; }
    }
}
