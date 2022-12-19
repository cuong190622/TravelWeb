using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelWeb.Models.DTO
{
    public class BookingRoomDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAdress { get; set; }
        public string RoomNo { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        public string HotelName { get; set; }



    }
}