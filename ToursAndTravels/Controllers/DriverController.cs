using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ToursAndTravels.Data;
using ToursAndTravels.Models;

namespace ToursAndTravels.Controllers
{
    public class DriverController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DriverController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ 1. List All Drivers
        public async Task<IActionResult> Index()
        {
            var drivers = await _context.Drivers.Include(d => d.Salaries).ToListAsync();
            return View(drivers);
        }

        // ✅ 2. Show Driver Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var driver = await _context.Drivers.Include(d => d.Salaries).FirstOrDefaultAsync(d => d.DriverId == id);

            if (driver == null) return NotFound();

            return View(driver);
        }

        // ✅ 3. Create - GET
        public IActionResult Create()
        {
            return View(new Driver());
        }

        // ✅ 4. Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Driver driver)
        {
            if (ModelState.IsValid)
            {
                _context.Drivers.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        // ✅ 5. Edit - GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null) return NotFound();

            return View(driver);
        }

        // ✅ 6. Edit - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Driver driver)
        {
            if (id != driver.DriverId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.DriverId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        // ✅ 7. Delete - GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null) return NotFound();

            return View(driver);
        }

        // ✅ 8. Delete - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver != null)
            {
                _context.Drivers.Remove(driver);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DriverExists(int id)
        {
            return _context.Drivers.Any(e => e.DriverId == id);
        }
    }
}
