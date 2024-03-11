using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace _AbsoPickUp.Models
{
    public class ParcelSubCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubCategoryId { get; set; }
        [ForeignKey("ParcelCategory")]
        [Required]
        public int CategoryId { get; set; }
        [MaxLength(250)]
        [Required]
        public string Description { get; set; }

        public ParcelCategory ParcelCategory { get; set; }
    }
}
