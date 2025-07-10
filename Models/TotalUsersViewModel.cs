using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NestFinder.Models
{
    public class TotalUsersViewModel
    {
        public string Id { get; set; } // User ID
        public string FullName { get; set; } // Name
        public string Email { get; set; } // Email
        public DateTime JoinDate { get; set; } // Join Date
        public string City { get; set; } // City
    }
}