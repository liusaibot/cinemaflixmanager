using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CinemaFlixManager.Models;
using CinemaFlixManager.Data;
using Microsoft.EntityFrameworkCore;
using CinemaFlixManager.Models.ShowTimeViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace CinemaFlixManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            try
            {

                var model = await _context.Movies
                                .Where(x => x.IsCurrentlyShowing == true)
                                .OrderByDescending(x => x.Id)
                                .ToListAsync();

            var showings = await _context.MovieShowings.ToListAsync();
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "Id", "Name");

            List<ShowTimeViewModel> listShowTime = new List<ShowTimeViewModel>();
            
            foreach(var showing in showings ?? Enumerable.Empty<MovieShowings>())
            {
                if(showing == null)
                {

                }
                else
                {
                    ShowTimeViewModel showTime = new ShowTimeViewModel()
                    {
                        title = showing.Movie.Title,
                        start = showing.ShowTime.ToString(),
                        end = showing.ShowTime.Value.AddMinutes(showing.Movie.Duration).ToString(),
                        backgroundColor = showing.Movie.showColour,
                        borderColor = showing.Movie.showColour
                    };

                    listShowTime.Add(showTime);
                }
                
            }

            ViewBag.showtimejSON = JsonConvert.SerializeObject(listShowTime);

            HomePageViewModel homePageViewModel = new HomePageViewModel()
            {
                Movies = model,
                ShowTimeViewModels = listShowTime
            };

            return View(homePageViewModel);
        }
            catch(NullReferenceException ex)
            {
                throw ex;
            }
}

        public async Task<IActionResult> ListAvailableMovies(List<Movie> model)
        {
            

            return PartialView("_ListAvailableMovies", model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
