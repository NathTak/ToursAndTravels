using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToursAndTravels.Data;
using System;
using System.Threading.Tasks;

namespace ToursAndTravels
{
    public class Program
    {
        public static async Task Main(string[] args) // ? Async method ??? ?? implement ???? ???
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database Connection
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity System
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add MVC and Razor Pages
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Enable Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Define Routes
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.MapRazorPages();

            // ?? Admin User Seed ???? ?? ??? Scope Create ????
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    await DbInitializer.SeedAdminUser(userManager); // ? Await Async Method ?? ????
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding admin user: {ex.Message}");
                }
            }

            await app.RunAsync(); // ? Async Method ?? ???? Run ???? ?????
        }
    }
}
