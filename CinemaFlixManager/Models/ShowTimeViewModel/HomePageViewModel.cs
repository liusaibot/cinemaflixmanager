using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaFlixManager.Models.ShowTimeViewModel
{
    public class HomePageViewModel
    {
        public List<ShowTimeViewModel> ShowTimeViewModels { get; set; }

        public List<Movie> Movies { get; set; }
    }
}
