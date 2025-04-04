using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToursAndTravels.Data;
using ToursAndTravels.Models;
using System.Threading.Tasks;

namespace ToursAndTravels.Controllers
{
    public class TripController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TripController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trips
        public IActionResult Index()
        {
            var trips = _context.Trips
                .Select(t => new Trip  // ❌ Anonymous type mat return karo, Trip model return karo
                {
                    TripId = t.TripId,
                    Date = t.Date,
                    VehicleNo = t.VehicleNo,
                    FromLocation = t.FromLocation,
                    ToLocation = t.ToLocation,
                    TripRent = t.TripRent
                }).ToList();

            return View(trips);
        }


        // GET: Trips/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trips/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,VehicleNo,FromLocation,ToLocation,TripRent")] Trip trip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trip);
        }


        // GET: Trips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null) return NotFound();
            return View(trip);
        }

        // POST: Trips/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TripId,Date,VehicleNo,FromLocation,ToLocation,TripRent")] Trip trip)
        {
            if (id != trip.TripId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(trip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trip);
        }

        // GET: Trips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var trip = await _context.Trips.FirstOrDefaultAsync(m => m.TripId == id);
            if (trip == null) return NotFound();
            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip != null)
            {
                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
