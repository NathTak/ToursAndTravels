using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ToursAndTravels.Data
{
    public class DbInitializer
    {
        public static async Task SeedAdminUser(UserManager<IdentityUser> userManager)
        {
            if (!userManager.Users.Any()) // Check if users exist
            {
                var adminUser = new IdentityUser
                {
                    UserName = "admin@tours.com",
                    Email = "admin@tours.com",
                    EmailConfirmed = true
                };

                string adminPassword = "Admin@123"; // Strong Password

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    Console.WriteLine("Admin User Created Successfully!");
                }
                else
                {
                    Console.WriteLine("Error Creating Admin User!");
                }
            }
        }
    }
}
