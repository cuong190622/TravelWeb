using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TravelWeb.Models.DTO
{
    public class BookingTypeHtDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAdress { get; set; }
        [Display(Name = "Phone")]
        public string CustomerPhone { get; set; }
        [Display(Name = "From")]
        public DateTime BookingFrom { get; set; }
        [Display(Name = "To")]
        public DateTime BookingTo { get; set; }
        [Display(Name = "Member")]
        public int NumberOfMember { get; set; }
        public string Types { get; set; }
        public string Statuses { get; set; }
        [Display(Name = "Hotel")]
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