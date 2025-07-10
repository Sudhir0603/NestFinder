using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NestFinder.Models
{
    public class TotalPropertiesViewModel
    {
        public int Id { get; set; } // Property ID
        public string Title { get; set; } // Property Name
        public string Address { get; set; } // Property Address
        public string City { get; set; } // City
        public string State { get; set; } // State
        public decimal Price { get; set; } // Rent Price
        public string OwnerName { get; set; } // Owner Name
        public bool IsApproved { get; set; } // Approval Status
    }
}