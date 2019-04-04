using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaFlixManager.Data;
using CinemaFlixManager.Models;
using CinemaFlixManager.Models.ShowTimeViewModel;

namespace CinemaFlixManager.Controllers
{
    public class MovieShowingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieShowingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MovieShowings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MovieShowings
                                                .Include(m => m.Cinema)
                                                .Include(m => m.Movie)
                                                .OrderBy(m => m.ShowTime)
                                                .OrderByDescending(s => s.ShowingDate);
                                                
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> List()
        {
            var applicationDbContext = _context.MovieShowings
                                                .Include(m => m.Cinema)
                                                .Include(m => m.Movie)
                                                .OrderBy(m => m.ShowTime)
                                                .OrderByDescending(s => s.ShowingDate);
                                                

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MovieShowings/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieShowings = await _context.MovieShowings
                .Include(m => m.Cinema)
                .Include(m => m.Movie)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (movieShowings == null)
            {
                return NotFound();
            }

            return View(movieShowings);
        }

        // GET: MovieShowings/Create
        public IActionResult Create()
        {
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            return View();
        }

        // POST: MovieShowings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CinemaId,MovieId,ShowingDate,ShowTime,DateCreated,DateUpdated")] MovieShowings movieShowings)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movieShowings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Id", movieShowings.CinemaId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", movieShowings.MovieId);
            return View(movieShowings);
        }

        public IActionResult AddShowTimes()
        {
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShowTimes(AddShowTimeViewModel movieShowings)
        {
            List<MovieShowings> showingsList = new List<MovieShowings>();

            if (ModelState.IsValid)
            {
                

                char dateDelimiter = '-';
                string[] dates = movieShowings.ShowingDates.Split(dateDelimiter);

                var allDates = GetDatesBetween(Convert.ToDateTime(dates[0]), Convert.ToDateTime(dates[1]));

                char timeDelimeter = ',';
                string[] showTimes = movieShowings.ShowingTimes.Split(timeDelimeter);

                try
                {

                    foreach (var date in allDates)
                    {
                        foreach (var showTime in showTimes)
                        {
                            TimeSpan timeSpan = TimeSpan.FromHours(Convert.ToInt32(showTime));
                            //string fromTimeSpan = timeSpan.ToString("hh':'mm':'ss");
                            //DateTime dateTime = DateTime.ParseExact(showTime, "HH:mm:ss",
                            //                CultureInfo.InvariantCulture);
                            var showing = new MovieShowings()
                            {
                                CinemaId = movieShowings.CinemaId,
                                MovieId = movieShowings.MovieId,
                                DateCreated = DateTime.Now,
                                DateUpdated = DateTime.Now,
                                ShowingDate = date.Date,
                                ShowTime = date.Date + timeSpan
                        };
                            //string dateconcat = date.Date + timeSpan;
                            
                            //showing.ShowTime = Convert.ToDateTime(dateconcat);
                            
                            showingsList.Add(showing);
                            _context.Entry(showing).State = EntityState.Added;
                        }
                    }
                    
                    //_context.Entry(showingsList);
                }
                catch(Exception ex)
                {
                    throw ex;
                }

                //foreach (var showing in showingsList)
                //{
                //    char delimiter = '-';
                //    string[] dates = movieShowings.ShowingDates.Split(delimiter);

                //}
               // _context.Add(showingsList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name", movieShowings.CinemaId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", movieShowings.MovieId);
            return View(movieShowings);
        }

        // GET: MovieShowings/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieShowings = await _context.MovieShowings.SingleOrDefaultAsync(m => m.Id == id);
            if (movieShowings == null)
            {
                return NotFound();
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Id", movieShowings.CinemaId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", movieShowings.MovieId);
            return View(movieShowings);
        }



        // POST: MovieShowings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CinemaId,MovieId,ShowingDate,ShowTime,DateCreated,DateUpdated")] MovieShowings movieShowings)
        {
            if (id != movieShowings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieShowings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieShowingsExists(movieShowings.Id))
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
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Id", movieShowings.CinemaId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", movieShowings.MovieId);
            return View(movieShowings);
        }

        // GET: MovieShowings/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieShowings = await _context.MovieShowings
                .Include(m => m.Cinema)
                .Include(m => m.Movie)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (movieShowings == null)
            {
                return NotFound();
            }

            return View(movieShowings);
        }

        private List<DateTime> GetDatesBetween(DateTime startDate, DateTime endDate)
        {
            List<DateTime> allDates = new List<DateTime>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                allDates.Add(date);
            return allDates;

        }

        // POST: MovieShowings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var movieShowings = await _context.MovieShowings.SingleOrDefaultAsync(m => m.Id == id);
            _context.MovieShowings.Remove(movieShowings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieShowingsExists(long id)
        {
            return _context.MovieShowings.Any(e => e.Id == id);
        }
    }
}
