using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class DeliveryPrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("DeliveryTypes")]
        public int TypeId { get; set; }
        public string DeliverBy { get; set; }
        public int Amount { get; set; }

        public DeliveryTypes DeliveryTypes { get; set; }
    }
}
