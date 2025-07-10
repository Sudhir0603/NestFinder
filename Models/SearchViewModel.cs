using System.Collections.Generic;

namespace NestFinder.Models
{
    public class SearchViewModel
    {
        public string Title { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Amenities { get; set; }
        public string SuitableFor { get; set; }
        public string Gender { get; set; }  // Boys, Girls, Both
        public int? MinRent { get; set; }  // Minimum Rent Filter
        public int? MaxRent { get; set; }  // Maximum Rent Filter

        // List to store search results
        public List<Property> SearchResults { get; set; }
        public List<int> FavoritePropertyIds { get; set; }
    }
}
