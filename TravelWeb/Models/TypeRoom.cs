using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TravelWeb.Models
{
    public class TypeRoom
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Need to input name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Need to input Desciption")]
        public string Description { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "Need to input MaxNumberRoom")]
        public int MaxNumberRoom { get; set; }
        [Required(ErrorMessage = "Need to input Price")]
        public float Price { get; set; }
        public int bookingId { get; set; }
        public List<Booking> listRoomsBookinig { get; set; }
        public int RoomActive { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

    }
}