using System.ComponentModel.DataAnnotations;

namespace NestFinder.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
    }
}
