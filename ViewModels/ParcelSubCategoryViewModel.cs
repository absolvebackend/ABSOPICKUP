using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class ParcelSubCategoryViewModel
    {
        public int SubCategoryId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [MaxLength(250)]
        [Required]
        public string Description { get; set; }
        public string ParcelCategory { get; set; }
    }
}
