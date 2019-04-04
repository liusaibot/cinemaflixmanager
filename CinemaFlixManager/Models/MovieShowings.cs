using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaFlixManager.Models
{
    public class MovieShowings
    {
        public long Id { get; set; }

        public Cinema Cinema { get; set; }

        public long CinemaId { get; set; }

        public Movie Movie { get; set; }

        public long MovieId { get; set; }

        public Nullable<DateTime> ShowingDate { get; set; }

        public Nullable<DateTime> ShowTime { get; set; }

        public Nullable<DateTime> DateCreated { get; set; }

        public Nullable<DateTime> DateUpdated { get; set; }

        public string ShowDate
        {
            get {
                return ShowingDate.Value.ToString("MMM dd',' yyyy"); }
        }

        public string ShowingAt
        {
            get
            {
                if (!string.IsNullOrEmpty(ShowTime.ToString()))
                {
                    return ShowTime.Value.ToString("hh:mm tt");
                }
                else
                {
                    return null;
                }
                
            }
        }
    }
}
