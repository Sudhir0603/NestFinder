using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Data.Entity;

namespace NestFinder.Models
{
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // ✅ Ensure "Admin" Role Exists
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole("Admin"));
            }

            // ✅ Ensure Admin User Exists
            if (!context.Users.Any(u => u.Email == "admin@nestfinder.com"))
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@nestfinder.com",
                    Email = "admin@nestfinder.com",
                    EmailConfirmed = true
                };

                var result = userManager.Create(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    userManager.AddToRole(adminUser.Id, "Admin");
                }
            }

            base.Seed(context);
        }
    }
}
