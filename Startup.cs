using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using NestFinder.Models; // Ensure this matches your actual namespace
using System;

[assembly: OwinStartupAttribute(typeof(NestFinder.Startup))]
namespace NestFinder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateAdminUserAndRole(); // ✅ Ensure Admin User is created with a valid password hash
        }

        private void CreateAdminUserAndRole()
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                string adminRole = "Admin";
                string adminEmail = "nestfinder2026@gmail.com";
                string adminPassword = "AdminHa@123"; // 🔒 Change this for security

                // ✅ Ensure the Admin Role exists
                if (!roleManager.RoleExists(adminRole))
                {
                    roleManager.Create(new IdentityRole(adminRole));
                }

                // ✅ Check if Admin User exists
                var adminUser = userManager.FindByEmail(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = "Admin",
                        Email = adminEmail,
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString(),

                        // ✅ Fix: Set Valid Date Values
                        LockoutEndDateUtc = null, // If required, use DateTime.UtcNow.AddYears(1)
                        JoinDate = DateTime.UtcNow, // Use a valid date
                    };

                    var result = userManager.Create(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        userManager.AddToRole(adminUser.Id, adminRole);
                    }
                    else
                    {
                        Console.WriteLine("Error creating admin user: " + string.Join(", ", result.Errors));
                    }
                }
            }
        }


    }
}
