using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToursAndTravels.Data;
using ToursAndTravels.Models;

namespace ToursAndTravels.Controllers
{
    public class TripExpenseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TripExpenseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ 1. Display All Expenses
        public IActionResult Index()
        {
            var expenses = _context.TripExpenses.Include(e => e.Trip).ToList();
            return View(expenses);
        }

        // ✅ 2. Show Expense Details
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var expense = _context.TripExpenses.Include(e => e.Trip).FirstOrDefault(e => e.ExpenseID == id);
            if (expense == null)
                return NotFound();

            return View(expense);
        }

        // ✅ 3. Create GET - Show Empty Form
        public IActionResult Create()
        {
            return View(new TripExpense()); // Ensures a new instance is passed
        }

        // ✅ 4. Create POST - Save Data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TripExpense expense)
        {
            if (ModelState.IsValid)
            {
                expense.CalculateTotalExpense(); // 🆕 Total Expense Auto-Calculate

                _context.TripExpenses.Add(expense);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }


        // ✅ 5. Edit GET - Show Existing Record
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tripExpense = await _context.TripExpenses.FindAsync(id);
            if (tripExpense == null) return NotFound();

            ViewBag.Trips = new SelectList(_context.Trips, "TripID", "VehicleNo", tripExpense.TripID);
            return View(tripExpense);
        }

        // ✅ 6. Edit POST - Update Data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TripExpense expense)
        {
            if (id != expense.ExpenseID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    expense.CalculateTotalExpense(); // 🆕 Total Expense Auto-Calculate

                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.TripExpenses.Any(e => e.ExpenseID == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }


        // ✅ 7. Delete GET - Confirm Before Deletion
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tripExpense = await _context.TripExpenses
                .Include(te => te.Trip)
                .FirstOrDefaultAsync(m => m.ExpenseID == id);

            if (tripExpense == null) return NotFound();

            return View(tripExpense);
        }

        // ✅ 8. Delete POST - Remove Data
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tripExpense = await _context.TripExpenses.FindAsync(id);
            if (tripExpense != null)
            {
                _context.TripExpenses.Remove(tripExpense);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
