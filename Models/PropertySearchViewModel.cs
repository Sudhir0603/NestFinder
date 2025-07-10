using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NestFinder.Models
{
    public class PropertySearchViewModel
    {
        public string SearchTerm { get; set; }  // Search by Title or Address

        public string City { get; set; }  // Filter by City

        [Display(Name = "Min Price")]
        public int? MinPrice { get; set; }  // Minimum Price

        [Display(Name = "Max Price")]
        public int? MaxPrice { get; set; }  // Maximum Price

        public string RoomType { get; set; }  // Filter by Room Type

        public string FurnishingStatus { get; set; }  // Fully Furnished, Semi-Furnished, etc.

        public string Gender { get; set; }  // Boys, Girls, Both

        public List<Property> Properties { get; set; }  // List of Filtered Properties
    }
}
