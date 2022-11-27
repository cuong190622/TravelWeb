using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelWeb.Models
{
    public class BookingRoom
    {
        public int Id { get; set; }
        public int BookingId { get; set; }

        public int RoomId { get; set; }
    }
}