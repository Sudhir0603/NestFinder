using System.Linq;
using System.Web.Mvc;
using NestFinder.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace NestFinder.Controllers
{
 //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ✅ View Pending Properties for Approval
        public ActionResult PendingProperties()
        {
            var properties = db.Properties.Where(p => !p.IsApproved).ToList();
            return View(properties);
        }

        // ✅ Approve Property
        public ActionResult Approve(int id)
        {
            var property = db.Properties.Find(id);
            if (property != null)
            {
                property.IsApproved = true;
                db.SaveChanges();
            }
            return RedirectToAction("PendingProperties");
        }

        // ✅ Reject and Delete Property
        public ActionResult Reject(int id)
        {
            var property = db.Properties.Find(id);
            if (property != null)
            {
                db.Properties.Remove(property);
                db.SaveChanges();
            }
            return RedirectToAction("PendingProperties");
        }

        // ✅ View All Users
        public ActionResult ManageUsers()
        {
            var users = db.Users.ToList();
            return View(users);
        }

        // ✅ Assign User as Admin
        public ActionResult MakeAdmin(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            userManager.AddToRole(id, "Admin");
            return RedirectToAction("ManageUsers");
        }


        public ActionResult Dashboard()
        {
            ViewBag.UsersCount = db.Users.Count();
            ViewBag.PropertiesCount = db.Properties.Count();
            ViewBag.PendingApprovals = db.Properties.Count(p => !p.IsApproved);

            return View();
        }
        // Property Details for Admin
        public ActionResult PropertyDetails(int id)
        {
            var property = db.Properties.Find(id);
            if (property == null) return HttpNotFound();
            return View(property);
        }

        // API to Approve or Reject Property
        [HttpPost]
        public JsonResult UpdateApprovalStatus(int propertyId, bool isApproved)
        {
            var property = db.Properties.Find(propertyId);
            if (property == null) return Json(new { success = false, message = "Property not found" });

            property.IsApproved = isApproved;
            db.SaveChanges();

            return Json(new { success = true });
        }
        public ActionResult TotalProperties()
        {
            var properties = db.Properties.Include(p => p.User).ToList();
            return View(properties);
        }
        [HttpPost]
        public JsonResult ApproveProperty(int propertyId)
        {
            var property = db.Properties.Find(propertyId);
            if (property == null) return Json(new { success = false, message = "Property not found" });

            property.IsApproved = true;
            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult DeleteProperty(int propertyId)
        {
            var property = db.Properties.Find(propertyId);
            if (property == null) return Json(new { success = false, message = "Property not found" });

            db.Properties.Remove(property);
            db.SaveChanges();
            return Json(new { success = true });
        }

        // GET: Admin/ViewDetails/{id}
        public ActionResult ViewDetails(int id)
        {
            var property = db.Properties
                .Include(p => p.User)
                .FirstOrDefault(p => p.Id == id);

            if (property == null)
            {
                return HttpNotFound();
            }

            return View(property);
        }
        public ActionResult TotalUsers()
        {
            var users = db.Users.Select(u => new TotalUsersViewModel
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                JoinDate = u.JoinDate,
                City = u.City
            }).ToList();

            return View(users); // Pass as strongly typed model
        }
       



        //  Remove Admin Role
        public ActionResult RemoveAdmin(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            userManager.RemoveFromRole(id, "Admin");
            return RedirectToAction("ManageUsers");
        }



        public ActionResult BlockUser(string id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                user.IsBlocked = true;
                db.SaveChanges();
            }
            return RedirectToAction("ManageUsers");
        }

        public ActionResult UnblockUser(string id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                user.IsBlocked = false;
                db.SaveChanges();
            }
            return RedirectToAction("ManageUsers");
        }

        public ActionResult DeleteUser(string id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                // 1️ Remove user's properties
                var properties = db.Properties.Where(p => p.UserId == id).ToList();
                db.Properties.RemoveRange(properties);

                // 2️ Remove user's contacts (if applicable)
                var contacts = db.Contacts.Where(c => c.Email == user.Email).ToList();
                db.Contacts.RemoveRange(contacts);

                // 3️ Remove user from roles (Identity)
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var roles = userManager.GetRoles(user.Id);
                foreach (var role in roles)
                {
                    userManager.RemoveFromRole(user.Id, role);
                }

                // 4️ Now, delete the user
                db.Users.Remove(user);

                // 5️ Save changes
                db.SaveChanges();
            }

            return RedirectToAction("ManageUsers");
        }

        
       

        public ActionResult Report()
        {
            ViewBag.UsersCount = db.Users.Count();
            ViewBag.PropertiesCount = db.Properties.Count();
            ViewBag.PendingApprovals = db.Properties.Count(p => !p.IsApproved);
            ViewBag.ApprovedProperties = db.Properties.Count(p => p.IsApproved);
            ViewBag.PendingProperties = db.Properties.Count(p => !p.IsApproved);

            // Get top users who posted most properties
            var topUsers = db.Properties
                .GroupBy(p => p.UserId)
                .Select(g => new TopUserViewModel
                {
                    UserId = g.Key,
                    Name = db.Users.Where(u => u.Id == g.Key).Select(u => u.FullName).FirstOrDefault(), // ✅ Fetch Name
                    PropertyCount = g.Count()
                })
                .OrderByDescending(g => g.PropertyCount)
                .Take(5)
                .ToList();

            ViewBag.TopUsers = topUsers;
            return View(topUsers); // Pass as a strongly-typed model
        }
    }
}
