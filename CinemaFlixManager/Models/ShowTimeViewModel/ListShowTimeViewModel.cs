using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaFlixManager.Models.ShowTimeViewModel
{
    public class ListShowTimeViewModel
    {
        public List<MovieShowings> MovieShowings { get; set; }

        public Movie Movie { get; set; }
    }
}
