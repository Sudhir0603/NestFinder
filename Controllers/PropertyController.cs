using System.Linq;
using System.Web.Mvc;
using NestFinder.Models;
using System.Data.Entity;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Microsoft.AspNet.Identity;
using System;

namespace NestFinder.Controllers
{
    public class PropertyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

     
        //  View All Approved Properties
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();

            var properties = db.Properties.Where(p => p.IsApproved).ToList();

            var favoritePropertyIds = db.FavoriteProperties
                .Where(f => f.UserId == userId)
                .Select(f => f.PropertyId)
                .ToList();

            ViewBag.FavoritePropertyIds = favoritePropertyIds;

            return View(properties);
        }

        public ActionResult MyFavorites()
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var favoritePropertyIds = db.FavoriteProperties
                .Where(f => f.UserId == userId)
                .Select(f => f.PropertyId)
                .ToList();

            var favoriteProperties = db.Properties
                .Where(p => favoritePropertyIds.Contains(p.Id))
                .ToList();

            return View(favoriteProperties);
        }

        // Add Property to Favorites
        [HttpPost]
        public JsonResult AddToFavorites( FavoriteProperty favoriteData)
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = " not logged in!" });
            }

            // Check if already in favorites
            var existingFavorite = db.FavoriteProperties
                .FirstOrDefault(f => f.PropertyId == favoriteData.PropertyId && f.UserId == userId);

            if (existingFavorite == null)
            {
                var favorite = new FavoriteProperty
                {
                    PropertyId = favoriteData.PropertyId,
                    UserId = userId
                };

                db.FavoriteProperties.Add(favorite);
                db.SaveChanges();

                return Json(new { success = true, message = "Added to favorites ✅" });
            }

            return Json(new { success = false, message = "Already in favorites" });
        }


        // ✅ Remove Property from Favorites
        [HttpPost]
        public JsonResult RemoveFromFavorites(FavoriteProperty favoriteData)
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = " not logged in!" });
            }

            var favorite = db.FavoriteProperties
                .FirstOrDefault(f => f.PropertyId == favoriteData.PropertyId && f.UserId == userId);

            if (favorite != null)
            {
                db.FavoriteProperties.Remove(favorite);
                db.SaveChanges();
                return Json(new { success = true, message = "Removed from favorites ❌" });
            }

            return Json(new { success = false, message = "Property not in favorites" });
        }


       
        public ActionResult Create()

        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

       

        // POST: Post Property
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Property property, HttpPostedFileBase[] ImageFiles, List<string> SuitableFor, List<string> Amenities, List<string> Rules, List<Room> Rooms)
        {
           

            if (ModelState.IsValid)
            {
                if (ImageFiles != null && ImageFiles.Length > 0)
                {
                    string imagePaths = "";
                    foreach (var file in ImageFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string fileName = Path.GetFileName(file.FileName);
                            string path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                            file.SaveAs(path);
                            imagePaths += "/Uploads/" + fileName + ";";
                        }
                    }
                    property.ImageUrls = imagePaths.TrimEnd(';');
                }

                // Convert multiple checkbox values to semicolon-separated strings
                property.SuitableFor = SuitableFor != null && SuitableFor.Any() ? string.Join(";", SuitableFor) : "";
                property.Amenities = Amenities != null && Amenities.Any() ? string.Join(";", Amenities) : "";
                property.Rules = Rules != null && Rules.Any() ? string.Join(";", Rules) : "";

                property.IsApproved = false;
                property.UserId = User.Identity.GetUserId();
               
                // Save Rooms Linked to Property
               
                db.Properties.Add(property);
                db.SaveChanges();

                return RedirectToAction("AddRoom", "Room", new { propertyId = property.Id });
            }
            
            return View(property);
        }

        // GET: Property Details
        public ActionResult Details(int id)
        {
            var property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        //// Edit Property (GET)
        public ActionResult Edit(int id)
        {
            string userId = User.Identity.GetUserId();
            var property = db.Properties.FirstOrDefault(p => p.Id == id && p.UserId == userId);

            if (property == null)
            {
                return HttpNotFound();
            }

            return View(property);
        }

        // Edit Property (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Property model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();  // Get the User ID first

                var property = db.Properties.FirstOrDefault(p => p.UserId == userId); // Now use it in the query


                if (property == null)
                {
                    return HttpNotFound();
                }

                property.Title = model.Title;
                property.Description = model.Description;
                property.Address = model.Address;
                property.City = model.City;
                property.State = model.State;
                property.ZipCode = model.ZipCode;
                property.Price = model.Price;
                property.RoomType = model.RoomType;
                property.FurnishingStatus = model.FurnishingStatus;
                property.Amenities = model.Amenities;
                property.SuitableFor = model.SuitableFor;
                property.NoticePeriod = model.NoticePeriod;

                db.SaveChanges();
                return RedirectToAction("MyProperties");
            }

            return View(model);
        }

     

        //GET: Search Propertieshtt
      
        public ActionResult Search()
        {
            var model = new SearchViewModel();  // Always initialize the model
            return View(model);
        }



        [HttpPost]
        public ActionResult Search(SearchViewModel model)
        {
            if (model == null) return View(new SearchViewModel());

            var properties = db.Properties.AsQueryable();

            if (!string.IsNullOrEmpty(model.Title))
                properties = properties.Where(p => p.Title.Contains(model.Title));

            if (!string.IsNullOrEmpty(model.City))
                properties = properties.Where(p => p.City.Contains(model.City));

            if (!string.IsNullOrEmpty(model.State))
                properties = properties.Where(p => p.State.Contains(model.State));

            if (!string.IsNullOrEmpty(model.Amenities))
                properties = properties.Where(p => p.Amenities.Contains(model.Amenities));

            if (!string.IsNullOrEmpty(model.SuitableFor))
                properties = properties.Where(p => p.SuitableFor.Contains(model.SuitableFor));

            if (!string.IsNullOrEmpty(model.Gender))
                properties = properties.Where(p => p.Gender == model.Gender);

            if (model.MinRent.HasValue)
                properties = properties.Where(p => p.Price >= model.MinRent.Value);

            if (model.MaxRent.HasValue)
                properties = properties.Where(p => p.Price <= model.MaxRent.Value);

            var userId = User.Identity.GetUserId();
            var favoritePropertyIds = db.FavoriteProperties
                                        .Where(f => f.UserId == userId)
                                        .Select(f => f.PropertyId)
                                        .ToList();

            model.SearchResults = properties.ToList();
            ViewBag.FavoritePropertyIds = favoritePropertyIds;

            return View(model);
        }



        

        // GET: Approve Property (Admin Only)
        [Authorize(Roles = "Admin")]
        public ActionResult Approve(int id)
        {
            var property = db.Properties.Find(id);
            if (property != null)
            {
                property.IsApproved = true;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        

       
        public ActionResult MyProperties()
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
            //string userId = User.Identity.GetUserId(); // Get the logged-in user ID
            var userProperties = db.Properties.Where(p => p.UserId == userId).ToList();
            return View(userProperties);
        }

        [HttpPost]
        public JsonResult DeleteProperty(int id)
        {

            try
            {
                var property = db.Properties.Find(id);
                if (property == null)
                {
                    return Json(new { success = false, message = "Property not found!" });
                }

                db.Properties.Remove(property);
                db.SaveChanges();

                return Json(new { success = true, message = "Property deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }



        public ActionResult FilteredProperties(string gender, string suitableFor)
        {
            var properties = db.Properties.AsQueryable();

            if (!string.IsNullOrEmpty(gender) && gender != "Any")
            {
                properties = properties.Where(p => p.Gender == gender);
            }

            if (!string.IsNullOrEmpty(suitableFor) && suitableFor != "Any")
            {
                properties = properties.Where(p => p.SuitableFor == suitableFor);
            }

            ViewBag.SelectedGender = gender;
            ViewBag.SelectedSuitableFor = suitableFor;

            return View(properties.ToList());
        }


    }
}
