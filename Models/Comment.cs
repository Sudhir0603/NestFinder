using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestFinder.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PropertyId { get; set; }  // Link to Property

        [ForeignKey("PropertyId")]
        public virtual Property Property { get; set; }

        [Required]
        public string UserId { get; set; }  // User who commented

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [StringLength(500)] // Limit comment length
        public string Content { get; set; }

        public int Stars { get; set; }  // Rating given

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
