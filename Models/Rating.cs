using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestFinder.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PropertyId { get; set; } // Linked Property ID

        [ForeignKey("PropertyId")]
        public virtual Property Property { get; set; }

        [Required]
        public string UserId { get; set; } // User who rated

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Range(1, 5)] // Rating between 1 and 5
        public int Stars { get; set; }

        public DateTime DateRated { get; set; } = DateTime.Now;
    }
}
