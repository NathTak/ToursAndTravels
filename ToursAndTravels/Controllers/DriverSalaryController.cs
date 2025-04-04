using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ToursAndTravels.Data;
using ToursAndTravels.Models;

namespace ToursAndTravels.Controllers
{
    public class DriverSalaryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DriverSalaryController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        // ✅ Display All Salaries
        public async Task<IActionResult> Index()
        {
            var salaries = await _context.DriverSalaries
                .Include(d => d.Driver)
                .ToListAsync();
            return View(salaries);
        }

        // ✅ Show Create Salary Form
        public IActionResult Create()
        {
            var drivers = _context.Drivers.ToList();

            if (drivers == null || !drivers.Any()) // Agar drivers null ya empty hai to error ayega
            {
                throw new Exception("No drivers found in the database. Please add drivers first.");
            }

            ViewBag.Drivers = drivers;
            return View();
        }


        // ✅ Add New Salary (POST)
        // ❌ Galat: [HttpPost] public async Task<IActionResult> AddSalary(DriverSalary model)
        // ✅ Sahi: [HttpPost] public async Task<IActionResult> Create(DriverSalary model)
        [HttpPost]
        public async Task<IActionResult> Create(DriverSalary model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Validation Error: " + error.ErrorMessage);
                }

                ViewBag.Drivers = _context.Drivers.ToList(); // Ensure drivers list
                return View(model);
            }

            _context.DriverSalaries.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        // ✅ Show Edit Form
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var salary = await _context.DriverSalaries.FindAsync(id);
            if (salary == null) return NotFound();

            ViewBag.Drivers = _context.Drivers.ToList();
            return View(salary);
        }

        // ✅ Edit Salary (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DriverSalary salary)
        {
            if (id != salary.SalaryId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Drivers = _context.Drivers.ToList();
                return View(salary);
            }

            try
            {
                _context.Update(salary);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.DriverSalaries.Any(e => e.SalaryId == id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ Show Delete Confirmation
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var salary = await _context.DriverSalaries
                .Include(d => d.Driver)
                .FirstOrDefaultAsync(m => m.SalaryId == id);
            if (salary == null) return NotFound();

            return View(salary);
        }

        // ✅ Delete Salary (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salary = await _context.DriverSalaries.FindAsync(id);
            if (salary != null)
            {
                _context.DriverSalaries.Remove(salary);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
