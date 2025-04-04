using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ToursAndTravels.Controllers
{
    [Authorize] // Ensure Only Logged-in Users Can Access
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
