using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaFlixManager.Data;
using CinemaFlixManager.Models;

namespace CinemaFlixManager.Controllers
{
    public class CinemaStatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CinemaStatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CinemaStates
        public async Task<IActionResult> Index()
        {
            return View(await _context.CinemaStates.ToListAsync());
        }

        // GET: CinemaStates/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinemaStates = await _context.CinemaStates
                .SingleOrDefaultAsync(m => m.Id == id);
            if (cinemaStates == null)
            {
                return NotFound();
            }

            return View(cinemaStates);
        }

        // GET: CinemaStates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CinemaStates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CinemaStates cinemaStates)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cinemaStates);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cinemaStates);
        }

        // GET: CinemaStates/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinemaStates = await _context.CinemaStates.SingleOrDefaultAsync(m => m.Id == id);
            if (cinemaStates == null)
            {
                return NotFound();
            }
            return View(cinemaStates);
        }

        // POST: CinemaStates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name")] CinemaStates cinemaStates)
        {
            if (id != cinemaStates.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cinemaStates);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CinemaStatesExists(Convert.ToInt32(cinemaStates.Id)))
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
            return View(cinemaStates);
        }

        // GET: CinemaStates/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinemaStates = await _context.CinemaStates
                .SingleOrDefaultAsync(m => m.Id == id);
            if (cinemaStates == null)
            {
                return NotFound();
            }

            return View(cinemaStates);
        }

        // POST: CinemaStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cinemaStates = await _context.CinemaStates.SingleOrDefaultAsync(m => m.Id == id);
            _context.CinemaStates.Remove(cinemaStates);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CinemaStatesExists(int id)
        {
            return _context.CinemaStates.Any(e => e.Id == id);
        }
    }
}
