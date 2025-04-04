using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ToursAndTravels.Data;
using ToursAndTravels.Models;

namespace ToursAndTravels.Controllers
{
    public class ProfitLossReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfitLossReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reports = await _context.Trips
                .Include(t => t.TripExpenses) // ✅ Expenses को लोड करें
                .Select(t => new ProfitLossReport
                {
                    TripId = t.TripId,
                    TripDate = t.Date,  // ✅ Model में Add किया गया
                    VehicleNo = t.VehicleNo,  // ✅ Model में Add किया गया
                    TripRent = t.TripRent,
                    TotalExpense = t.TripExpenses.Sum(e => e.Amount), // ✅ Corrected
                })
                .ToListAsync();

            return View(reports);
        }
    }
}
