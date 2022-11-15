using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TravelWeb.Models
{
    public class Location
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Need to input name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string LocationImage { get; set; }
        public List<Hotel> ListHotel { get; set; }
    }
}