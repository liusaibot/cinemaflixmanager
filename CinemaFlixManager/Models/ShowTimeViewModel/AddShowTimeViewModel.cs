using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaFlixManager.Models.ShowTimeViewModel
{
    public class AddShowTimeViewModel
    {
        [Required]
        public long MovieId { get; set; }

        [Required]
        public long CinemaId { get; set; }

        [Required]
        public string ShowingDates { get; set; }

        [Required]
        public string ShowingTimes { get; set; }
    }
}
