using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaFlixManager.Data;
using CinemaFlixManager.Models;
using CinemaFlixManager.Interfaces;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet.Actions;
using System.Net.Http.Headers;
using System.IO;
using CloudinaryDotNet;
using CinemaFlixManager.Models.ShowTimeViewModel;

namespace CinemaFlixManager.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            ViewBag.showImage = false;
            var model = await _context.Movies.OrderByDescending(x => x.Id).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> List()
        {
            var model = await _context.Movies.OrderByDescending(x => x.Id).ToListAsync();

            return PartialView("_List", model);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["Movie"] = new Movie();
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Synopsis,ThumbnailUrl,TrailerUrl,IsCurrentlyShowing,ReleaseDate,CreateDate,UpdateDate")] Movie movie)
        {
            movie.CreateDate = DateTime.Now;
            movie.UpdateDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            try
            {
                Stream fileStream = file.OpenReadStream();
                //filename = hostingEnv.WebRootPath + $@"\{filename}";
                var cloudinaryParams = new
                {
                    CLOUD_NAME = "dxf3lzbni",
                    API_KEY = "864775871664545",
                    API_SECRET = "9XuUadHYAieQeMiq_fvFN4egE3A"
                };
                Account account = new Account(cloudinaryParams.CLOUD_NAME, cloudinaryParams.API_KEY, cloudinaryParams.API_SECRET);
                Cloudinary cloudinary = new Cloudinary(account);
                ImageUploadParams uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, fileStream),

                };

                var uploadResult = cloudinary.Upload(uploadParams);

                //string url = cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));
                string url = cloudinary.Api.UrlImgUp.BuildUrl(uploadResult.SecureUri.ToString());
                string transformUrl = url.Substring(0, 50) + "w_100,h_150,c_fill/" + url.Substring(50);

                transformUrl = cloudinary.Api.UrlImgUp.BuildImageTag(transformUrl);

                var response = new
                {
                    url = url,
                    transformUrl = transformUrl
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IActionResult CreateShowing()
        {
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShowing([Bind("Id,CinemaId,MovieId,ShowingDate,ShowTime,DateCreated,DateUpdated")] MovieShowings movieShowings)
        {
            movieShowings.ShowTime = movieShowings.ShowingDate;
            movieShowings.DateCreated = DateTime.Now;
            movieShowings.DateUpdated = DateTime.Now;
            if (ModelState.IsValid)
            {
                
                _context.Add(movieShowings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Showings), new { id = movieShowings.MovieId });
            }
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Id", movieShowings.CinemaId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", movieShowings.MovieId);
            return View(movieShowings);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Synopsis,ThumbnailUrl,TrailerUrl,IsCurrentlyShowing,ReleaseDate,CreateDate,UpdateDate")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(Convert.ToInt32(movie.Id)))
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
            return View(movie);
        }

        public async Task<IActionResult> Showings(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            ViewData["MovieShowings"] = new MovieShowings()
            {
                MovieId = movie.Id
            };

            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            var showing = await _context.MovieShowings.Where(m => m.MovieId == movie.Id).OrderByDescending(m => m.ShowingDate).ToListAsync();

            ListShowTimeViewModel listShowTime = new ListShowTimeViewModel()
            {
                Movie = movie,
                MovieShowings = showing
            };
            return View(listShowTime);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movies = await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);
            if (movies == null)
            {
                return NotFound();
            }
            try
            {
                if (movies.IsCurrentlyShowing == true)
                {
                    movies.IsCurrentlyShowing = false;
                }
                else if (movies.IsCurrentlyShowing == false)
                {
                    movies.IsCurrentlyShowing = true;
                }
                _context.Update(movies);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(Convert.ToInt32(movies.Id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(movies);
        }

        

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
