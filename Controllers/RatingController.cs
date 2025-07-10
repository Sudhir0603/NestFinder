using System;
using System.Linq;
using System.Web.Mvc;
using NestFinder.Models;
using Microsoft.AspNet.Identity;

namespace NestFinder.Controllers
{
    public class RatingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public ActionResult SubmitRating(int propertyId, int stars)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
                return Json(new { success = false, message = "Login required" });

            var existingRating = db.Ratings.FirstOrDefault(r => r.PropertyId == propertyId && r.UserId == userId);

            if (existingRating != null)
            {
                existingRating.Stars = stars;
                existingRating.DateRated = DateTime.Now;
            }
            else
            {
                var newRating = new Rating
                {
                    PropertyId = propertyId,
                    UserId = userId,
                    Stars = stars
                };
                db.Ratings.Add(newRating);
            }

            db.SaveChanges();

            var avgRating = db.Ratings.Where(r => r.PropertyId == propertyId).Average(r => (double?)r.Stars) ?? 0;
            return Json(new { success = true, averageRating = avgRating });
        }

        // ✅ Fetch Overall Property Rating (Return 0 if no ratings)
        [HttpGet]
        public JsonResult GetAverageRating(int propertyId)
        {
            var ratings = db.Ratings.Where(r => r.PropertyId == propertyId).ToList();
            double avgRating = ratings.Any() ? ratings.Average(r => r.Stars) : 0;

            return Json(new { averageRating = avgRating }, JsonRequestBehavior.AllowGet);
        }
    }
}
