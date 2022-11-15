using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TravelWeb.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Need to input name")]
        public string Name { get; set; }
        public int Star { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Need to input Location")]
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string HotelImage { get; set; }


        public int TypeRoomId { get; set; }
        public List<TypeRoom> TypeRooms { get; set; }

    }
}