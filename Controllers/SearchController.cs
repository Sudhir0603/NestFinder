using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NestFinder.Models;

namespace NestFinder.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string Localities, string LookingFor, int? RentRange, string FurnishingStatus, bool? ImageFilter)
        {
            var properties = db.Properties.Where(p => p.IsApproved).AsQueryable(); // Fetch only approved properties

            if (!string.IsNullOrEmpty(Localities))
            {
                properties = properties.Where(p => p.Address.Contains(Localities) || p.City.Contains(Localities));
            }

            if (!string.IsNullOrEmpty(LookingFor))
            {
                properties = properties.Where(p => p.RoomType == LookingFor);
            }

            if (RentRange.HasValue)
            {
                properties = properties.Where(p => p.Rent <= RentRange);
            }

            if (!string.IsNullOrEmpty(FurnishingStatus))
            {
                properties = properties.Where(p => p.FurnishingStatus == FurnishingStatus);
            }

            if (ImageFilter == true)
            {
                properties = properties.Where(p => !string.IsNullOrEmpty(p.ImageUrls));
            }

            return View("Results", properties.ToList()); // Show filtered results
        }
    }
}
