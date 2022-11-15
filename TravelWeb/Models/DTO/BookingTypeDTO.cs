using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelWeb.Models.DTO
{
    public class BookingTypeDTO
    {
        public int Id { get; set; }
        public string NameType { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public int MaxNumberRoom { get; set; }
        public string Description { get; set; }
        public int NumberRoomBook { get; set; }


        public object RoomFree
        {
            get
            {
                return MaxNumberRoom - NumberRoomBook;
            }
        }
        public int HotelId { get; set; }
    }
}