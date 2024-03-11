using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class CardTypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CardTypeId { get; set; }
        [Required]
        [MaxLength(50)]
        public string CardTypeName { get; set; }
    }
}
