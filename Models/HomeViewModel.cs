using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NestFinder.Models
{
    public class HomeViewModel
    {
        public List<Property> RandomProperties { get; set; }
        public int WorkingProfessionalsCount { get; set; }
        public int StudentsCount { get; set; }
        public int BoysCount { get; set; }
        public int GirlsCount { get; set; }
        public int TotalProperties { get; set; }
    }

}