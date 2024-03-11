using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _AbsoPickUp.Models
{
    public class DriverDocuments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocId { get; set; }
        [ForeignKey("DocumentTypes")]
        public int DocTypeId { get; set; }
        public int DriverId { get; set; }
        public string DocImgPath { get; set; }
        public DocumentTypes DocumentTypes { get; set; }
    }
}
