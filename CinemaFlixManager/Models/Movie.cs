using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaFlixManager.Models
{
    public class Movie
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Synopsis { get; set; }

        public string ThumbnailUrl { get; set; }

        public string TrailerUrl { get; set; }

        public bool IsCurrentlyShowing { get; set; }

        public int Duration { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
        public Nullable<DateTime> ReleaseDate { get; set; }

        public Nullable<DateTime> CreateDate { get; set; }

        public Nullable<DateTime> UpdateDate { get; set; }

        public string showColour
        {
            get
            {
                if (IsCurrentlyShowing == true)
                {
                    return "success";
                }
                else
                {
                    return "danger";
                }
            }
        }

        public string shortSynopsis
        {
            get
            {
                var length = 100;
                if (Synopsis.Length > length)
                {
                    return Synopsis.Substring(0, length) + "...";
                }
                return Synopsis;
            }
        }

        public string thumbnail
        {
            get
            {
                if (!string.IsNullOrEmpty(ThumbnailUrl) && ThumbnailUrl.Length >=50)
                {
                    string transformUrl = ThumbnailUrl.Substring(0, 50) + "w_100,h_150,c_fill/" + ThumbnailUrl.Substring(50);
                    return transformUrl;
                }
                else
                {
                    return string.Empty;
                }
            }
            
        }

        public string TagColour
        {
            get
            {
                var lastNumber = Id % 10;
                switch (lastNumber)
                {
                    case 1:
                        return "aqua";
                    case 2:
                        return "blue";
                    case 3:
                        return "light-blue";
                    case 4:
                        return "teal";
                    case 5:
                        return "yellow";
                    case 6:
                        return "orange";
                    case 7:
                        return "green";
                    case 8:
                        return "lime";
                    case 9:
                        return "red";
                    case 0:
                        return "purple";
                    default:
                        return "muted";                     
                }
            }
        }
    }

}
