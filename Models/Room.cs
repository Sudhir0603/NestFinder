using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestFinder.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        public string RoomType { get; set; } // Private, Double, Triple, etc.
        public int Rent { get; set; }
        public int SecurityDeposit { get; set; }

        // Foreign Key linking to Property
        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public virtual Property Property { get; set; }
    }
}
