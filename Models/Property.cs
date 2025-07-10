using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace NestFinder.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        
        public string Title { get; set; }

        
        public string Description { get; set; }

        
        public string Address { get; set; }

       
        public string City { get; set; }

        
        public string State { get; set; }

        
        public string ZipCode { get; set; }

        public decimal Price { get; set; }

        public string ImageUrls { get; set; }

        public bool IsApproved { get; set; } = false;

        
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        [NotMapped]
        public HttpPostedFileBase[] ImageFiles { get; set; } // For file upload

        // Newly Added Fields
        public string Contact { get; set; }
        public int? Rent { get; set; }
        public string RoomType { get; set; }
        public string SharingInfo { get; set; } // E.g., "Shared By 2"
        //
        //public string VerificationStatus { get; set; } // E.g., "Verified" or "Pending"
        public string Amenities { get; set; } // Semicolon-separated list of amenities
        public string Rules { get; set; } // Semicolon-separated list of rules
        public string FurnishingStatus { get; set; } // E.g., "Fully Furnished"
        public string NoticePeriod { get; set; } // E.g., "1 Month"
        public string SuitableFor { get; set; } // E.g., "Students, Working Professionals"
        public string LocationLink { get; set; }

        public string Gender { get; set; }  // Boys, Girls, Both

        //public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
