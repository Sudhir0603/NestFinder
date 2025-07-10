using System;
using System.ComponentModel.DataAnnotations;

namespace NestFinder.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        public DateTime SentDate { get; set; } = DateTime.Now;
    }
}

