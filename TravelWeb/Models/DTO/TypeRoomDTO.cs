using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelWeb.Models.DTO
{
    public class TypeRoomDTO
    {
        public int TypeId { get; set; }
        public int HotelId { get; set; }

        public string NameType { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public int MaxNumberRoom { get; set; }

        public string HotelName { get; set; }
        public int? AvailableRoom { get; set; }
    }
}