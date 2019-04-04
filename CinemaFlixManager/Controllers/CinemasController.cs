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
    public class CinemasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CinemasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cinemas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cinemas.Include(c => c.CinemaState);
            ViewData["CinemaStatesId"] = new SelectList(_context.CinemaStates, "Id", "Name");
            return View(await applicationDbContext.Include(m => m.CinemaState).ToListAsync());
        }

        public async Task<IActionResult> List()
        {
            var applicationDbContext = _context.Cinemas.Include(c => c.CinemaState);
            ViewData["CinemaStatesId"] = new SelectList(_context.CinemaStates, "Id", "Name");
            var cinemas = await applicationDbContext.Include(m => m.CinemaState).ToListAsync();

            return PartialView("_List", cinemas);
        }

        // GET: Cinemas/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinemas
                .Include(c => c.CinemaState)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }

            return View(cinema);
        }

        // GET: Cinemas/Create
        public IActionResult Create()
        {
            ViewData["CinemaStatesId"] = new SelectList(_context.CinemaStates, "Id", "Name");
            ViewData["Cinema"] = new Cinema();
            return View();
        }

   

        // POST: Cinemas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LogoPath,WebsiteUrl,IsActive,Address,City,CinemaStatesId")] Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cinema);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CinemaStatesId"] = new SelectList(_context.CinemaStates, "Id", "Id", cinema.CinemaStatesId);
            return View(cinema);
        }

        // GET: Cinemas/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinemas.SingleOrDefaultAsync(m => m.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }
            ViewData["CinemaStatesId"] = new SelectList(_context.CinemaStates, "Id", "Id", cinema.CinemaStatesId);
            return View(cinema);
        }

        // POST: Cinemas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,LogoPath,WebsiteUrl,IsActive,Address,City,CinemaStatesId")] Cinema cinema)
        {
            if (id != cinema.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cinema);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CinemaExists(cinema.Id))
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
            ViewData["CinemaStatesId"] = new SelectList(_context.CinemaStates, "Id", "Id", cinema.CinemaStatesId);
            return View(cinema);
        }

        // GET: Cinemas/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinemas
                .Include(c => c.CinemaState)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }

            return View(cinema);
        }

        // POST: Cinemas/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(long id)
        //{
        //    var cinema = await _context.Cinemas.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Cinemas.Remove(cinema);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var cinema = await _context.Cinemas.SingleOrDefaultAsync(m => m.Id == id);
            _context.Cinemas.Remove(cinema);
            await _context.SaveChangesAsync();
            return Ok("Deleted");
        }

        [HttpPut, ActionName("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(long id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var cinema = await _context.Cinemas.SingleOrDefaultAsync(m => m.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }

            cinema.IsActive = !cinema.IsActive;

            _context.Entry(cinema).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CinemaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("It is success!");
        }

        private bool CinemaExists(long id)
        {
            return _context.Cinemas.Any(e => e.Id == id);
        }
    }
}
