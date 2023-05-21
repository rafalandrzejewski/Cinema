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

namespace Cinema.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reservation.Include(r => r.ApplicationUser).Include(r => r.Seance);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            ViewData["SeanceId"] = new SelectList(_context.Seance, "Id", "Id");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,SeanceId,ApplicationUserId,SeatNumbers")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                if (reservation.SeanceId == null || _context.Seance == null)
                {
                    return NotFound();
                }

                var seance = await _context.Seance
                    .Include(s => s.Movie)
                    .FirstOrDefaultAsync(m => m.Id == reservation.SeanceId);
                if (seance == null)
                {
                    return NotFound();
                }
                var seats = JsonConvert.DeserializeObject<List<Seat>>(seance.SeatsJsonObject);
                var reservationSeats = reservation.SeatNumbers.Split(",").ToList();
                foreach (var seat in reservationSeats)
                {
                    if (Int32.TryParse(seat, out int seatNumber) && seats.Any(x => x.Id == seatNumber && x.IsBooked == false))
                        seats.Where(x => x.Id == seatNumber).FirstOrDefault().IsBooked = true;
                    else
                        return NotFound();//zrobic blad
                }
                seance.SeatsJsonObject = JsonConvert.SerializeObject(seats);
                _context.Update(seance);

                await _context.SaveChangesAsync();
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", reservation.ApplicationUserId);
            ViewData["SeanceId"] = new SelectList(_context.Seance, "Id", "Id", reservation.SeanceId);
            return View(reservation);
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
                    if(reservationSeats.Contains(seat.Id.ToString()))
                        seat.IsBooked = false;
                }
                seance.SeatsJsonObject = JsonConvert.SerializeObject(seats);
                _context.Update(seance);

                _context.Reservation.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return (_context.Reservation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
