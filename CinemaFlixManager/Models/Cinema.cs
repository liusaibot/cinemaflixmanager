using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaFlixManager.Models
{
    public class Cinema
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string LogoPath { get; set; }

        public string WebsiteUrl { get; set; }

        public bool IsActive { get; set; }

        public string Address { get; set; }

        public string City { get; set; }


        public CinemaStates CinemaState { get; set; }

        [Display(Name = "State")]
        public long CinemaStatesId { get; set; }

        public string showColour
        {
            get
            {
                if (IsActive == true)
                {
                    return "success";
                }
                else
                {
                    return "default";
                }
            }
        }

        public string shortName
        {
            get
            {
                var length = 30;
                if (Name.Length > length)
                {
                    return Name.Substring(0, length) + "...";
                }
                return Name;
            }
        }

        public string ShortAddress
        {
            get
            {
                var length = 40;
                if (Address.Length > length)
                {
                    return Address.Substring(0, length) + "...";
                }
                return Address;
            }
        }

        public string ActivityStatus
        {
            get
            {
                var status = IsActive == true ? "Active" : "Inactive";
                return status;
            }
        }
    }
}
