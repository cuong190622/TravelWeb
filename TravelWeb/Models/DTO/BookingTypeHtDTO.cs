using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelWeb.Models.DTO
{
    public class BookingTypeHtDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAdress { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime BookingFrom { get; set; }
        public DateTime BookingTo { get; set; }
        public int NumberOfMember { get; set; }
        public string Types { get; set; }
        public string Statuses { get; set; }
        public string HotelName { get; set; }

        public float Price { get; set; }
        public int NumberRoomBook { get; set; }

        public object TotalPrice
    {
            get
            {
                return Price * NumberRoomBook ;
            }
        }


    }
}