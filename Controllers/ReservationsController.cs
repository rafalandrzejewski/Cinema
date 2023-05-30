using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using Cinema.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reservation
                .Include(r => r.ApplicationUser)
                .Include(r => r.Seance)
                .ThenInclude(s => s.Movie);
            return View(await applicationDbContext.ToListAsync());

        }
        public async Task<IActionResult> IndexUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var applicationDbContext = _context.Reservation
                .Include(r => r.ApplicationUser)
                .Include(r => r.Seance)
                    .ThenInclude(s => s.Movie)
                .Where(x => x.ApplicationUserId == user.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Seance)
                    .ThenInclude(s => s.Movie)
                .Include(r => r.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create(int? id)
        {
            var seancesWithMovies = _context.Seance.Include(s => s.Movie).ToList();
            ViewBag.Seances = new SelectList(seancesWithMovies, "Id", "Movie.Title");
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            // Load the Seance and its associated Seats
            var seance = _context.Seance
                                 .FirstOrDefault(s => s.Id == id);
            if (seance == null)
            {
                return NotFound();
            }

            // Deserialize Seats from the SeatsJsonObject
            var seats = JsonConvert.DeserializeObject<List<Seat>>(seance.SeatsJsonObject);
            // Load the list of Seats into the ViewBag
            ViewBag.AvailableSeats = seats.ToList();
            ViewBag.SeanceDate = seance.Date;
            ViewBag.SeanceDate2 = new DateTimeOffset(seance.Date).ToUnixTimeMilliseconds();
            var reservation = new Reservation { SeanceId = id ?? default(int) };
            return View(reservation);
            return View(id);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int seanceId, List<int> selectedSeats)
        {
            if (ModelState.IsValid)
            {
                var seance = _context.Seance
                                     .FirstOrDefault(s => s.Id == seanceId);
                if (seance == null)
                {
                    return NotFound();
                }

                var seats = JsonConvert.DeserializeObject<List<Seat>>(seance.SeatsJsonObject);
                var selectedSeatObjects = seats.Where(s => selectedSeats.Contains(s.Id)).ToList();
                if (selectedSeatObjects.Any(s => s.IsBooked))
                {
                    return Problem("Jedno lub kilka z wybranych miejsc jest już zarezerwowane!");
                }

                // Update the IsBooked status of the seats
                foreach (var seat in selectedSeatObjects)
                {
                    seat.IsBooked = true;
                }
                // Serialize the updated seats back into the SeatsJsonObject
                seance.SeatsJsonObject = JsonConvert.SerializeObject(seats);

                // Create the Reservation
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                Reservation reservation = new Reservation()
                {
                    Date = DateTime.Now,
                    ApplicationUser = user,
                    ApplicationUserId = user.Id,
                    Seance = seance,
                    SeanceId = seance.Id,
                    SeatNumbers = string.Join(",", selectedSeats),
                };

                _context.Add(reservation);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(IndexUser));
            }

            var user2 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", user2.Id);
            ViewData["SeanceId"] = new SelectList(_context.Seance, "Id", "Id", seanceId);

            // Load the list of Seats into the ViewBag again for redisplaying the form
            var seance2 = _context.Seance
                                  .FirstOrDefault(s => s.Id == seanceId);
            var seats2 = JsonConvert.DeserializeObject<List<Seat>>(seance2.SeatsJsonObject);
            ViewBag.AvailableSeats = seats2.Where(s => !s.IsBooked).ToList();

            return View(seanceId);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", reservation.ApplicationUserId);
            ViewData["SeanceId"] = new SelectList(_context.Seance, "Id", "Id", reservation.SeanceId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,SeanceId,ApplicationUserId,SeatNumbers")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", reservation.ApplicationUserId);
            ViewData["SeanceId"] = new SelectList(_context.Seance, "Id", "Id", reservation.SeanceId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.ApplicationUser)
                .Include(r => r.Seance)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservation'  is null.");
            }
            var reservation = await _context.Reservation.FindAsync(id);

            if (reservation != null)
            {
                var seance = await _context.Seance
                    .Include(s => s.Movie)
                    .FirstOrDefaultAsync(m => m.Id == reservation.SeanceId);
                if (seance == null)
                {
                    return NotFound();
                }
                var seats = JsonConvert.DeserializeObject<List<Seat>>(seance.SeatsJsonObject);
                var reservationSeats = reservation.SeatNumbers.Split(",").ToList();
                foreach (var seat in seats)
                {
                    if (reservationSeats.Contains(seat.Id.ToString()))
                        seat.IsBooked = false;
                }
                seance.SeatsJsonObject = JsonConvert.SerializeObject(seats);
                _context.Update(seance);

                _context.Reservation.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            if (User.IsInRole("SuperAdmin"))
                return RedirectToAction(nameof(Index));
            else
                return RedirectToAction(nameof(IndexUser));
        }

        private bool ReservationExists(int id)
        {
            return (_context.Reservation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
