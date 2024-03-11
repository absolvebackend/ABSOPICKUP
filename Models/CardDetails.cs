using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class CardDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CardId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int CardTypeId { get; set; }
        [Required]
        [MaxLength(50)]
        public long CardNumber { get; set; }
        [MaxLength(20)]
        public string ValidThru { get; set; }
        [MaxLength(100)]
        public string CardHolderName { get; set; }
    }
}
