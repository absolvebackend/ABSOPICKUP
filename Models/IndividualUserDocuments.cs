using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class IndividualUserDocuments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocId { get; set; }
        [ForeignKey("DocumentTypes")]
        public int DocTypeId { get; set; }
        public string IndividualUserId { get; set; }
        public string DocImgPath { get; set; }
        public DocumentTypes DocumentTypes { get; set; }
    }
}
