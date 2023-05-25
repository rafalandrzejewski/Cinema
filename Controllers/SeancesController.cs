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
    public class SeancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SeancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Seances
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Seance.Include(s => s.Movie);
            var seanceList = await applicationDbContext.ToListAsync();
            List<Seat> seats = new List<Seat>();
            foreach (var seance in seanceList)
            {
                seats = JsonConvert.DeserializeObject<List<Seat>>(seance.SeatsJsonObject);
                seance.FreeSeatCount = seats.Where(x => x.IsBooked == false).Count();
                seance.SeatsJsonObject = null;
            }
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> IndexUser()
        {
            var applicationDbContext = _context.Seance.Include(s => s.Movie);
            var seanceList = await applicationDbContext.ToListAsync();
            List<Seat> seats = new List<Seat>();
            foreach (var seance in seanceList)
            {
                seats = JsonConvert.DeserializeObject<List<Seat>>(seance.SeatsJsonObject);
                seance.FreeSeatCount = seats.Where(x => x.IsBooked == false).Count();
                seance.SeatsJsonObject = null;
            }
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Seances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Seance == null)
            {
                return NotFound();
            }

            var seance = await _context.Seance
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seance == null)
            {
                return NotFound();
            }
            List<Seat> seats = new List<Seat>();
            seats = JsonConvert.DeserializeObject<List<Seat>>(seance.SeatsJsonObject);
            seance.FreeSeatCount = seats.Where(x => x.IsBooked == false).Count();
            seance.SeatsJsonObject = null;
            return View(seance);
        }

        // GET: Seances/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movie, "Title", "Title");
            return View();
        }

        // POST: Seances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,MaxSeatCount,FreeSeatCount,SeatsJsonObject,MovieId")] SeanceDto seanceDto)
        {
            if (ModelState.IsValid)
            {
                var movie = _context.Movie.FirstOrDefault(x => x.Title == seanceDto.MovieId);
                List<Seat> seats = new List<Seat>();
                for (int i = 0; i < seanceDto.MaxSeatCount; i++)
                {
                    seats.Add(new Seat
                    {
                        Id = i + 1,
                        IsBooked = false,
                    });
                }
                var seance = new Seance
                {
                    MovieId=movie.Id,
                    Date= seanceDto.Date,
                    MaxSeatCount = seanceDto.MaxSeatCount,
                    FreeSeatCount = seanceDto.MaxSeatCount,
                    SeatsJsonObject = JsonConvert.SerializeObject(seats)
                };
                _context.Add(seance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", seanceDto.MovieId);
            return View(seanceDto);
        }

        // GET: Seances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Seance == null)
            {
                return NotFound();
            }

            var seance = await _context.Seance.FindAsync(id);
            if (seance == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", seance.MovieId);
            return View(seance);
        }

        // POST: Seances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,MaxSeatCount,FreeSeatCount,SeatsJsonObject,MovieId")] Seance seance)
        {
            if (id != seance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeanceExists(seance.Id))
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
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", seance.MovieId);
            return View(seance);
        }

        // GET: Seances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Seance == null)
            {
                return NotFound();
            }

            var seance = await _context.Seance
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seance == null)
            {
                return NotFound();
            }

            return View(seance);
        }

        // POST: Seances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Seance == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Seance'  is null.");
            }
            var seance = await _context.Seance.FindAsync(id);
            if (seance != null)
            {
                _context.Seance.Remove(seance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeanceExists(int id)
        {
            return (_context.Seance?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
