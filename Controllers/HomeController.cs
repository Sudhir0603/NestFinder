using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NestFinder.Models;

namespace NestFinder.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            // Get random properties (limit to 3)
            var randomProperties = db.Properties
                                     .OrderBy(p => Guid.NewGuid())
                                     .Take(5)
                                     .ToList();
            var totalProperties = db.Properties.Count();
            // Get property counts
            var model = new HomeViewModel
            {
                TotalProperties = totalProperties,
                RandomProperties = randomProperties,
                WorkingProfessionalsCount = db.Properties.Count(p => p.SuitableFor == "Working Professionals"),
                StudentsCount = db.Properties.Count(p => p.SuitableFor == "Students"),
                BoysCount = db.Properties.Count(p => p.Gender == "Boys"),
                GirlsCount = db.Properties.Count(p => p.Gender == "Girls")
            };

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

    }
}