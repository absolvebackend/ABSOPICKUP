using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace _AbsoPickUp.Models
{
    public class DriverRatings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RatingsId { get; set; }
        public int DriverId { get; set; }
        [MaxLength(256)]
        public string UserId { get; set; }
        public double Ratings { get; set; }
        public string UserComment { get; set; }
        public DateTime CommentedAt { get; set; }
    }
}
