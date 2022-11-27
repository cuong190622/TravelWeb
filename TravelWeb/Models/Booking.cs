using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TravelWeb.Models
{
    public class Booking
    {
        public int Id { get; set; }

        
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Customer Adress")]
        [Required(ErrorMessage = "Need to input Customer Adress")]
        public string CustomerAdress { get; set; }
        [Display(Name = "Customer Phone")]
        [Required(ErrorMessage = "Need to input Customer Phone")]
        public string CustomerPhone { get; set; }
        [Required(ErrorMessage = "Need to input Booking From")]
        [Display(Name = "Booking From")]       
       [DataType(DataType.Date)]
        public DateTime BookingFrom { get; set; }

        [Required(ErrorMessage = "Need to input Booking to")]
        [Display(Name = "Booking To")]
        [DataType(DataType.Date)]
        public DateTime BookingTo { get; set; }

        [Display(Name = "Type Room")]
        public int TypeRoomId { get; set; }

        [Required(ErrorMessage = "Need to input Number Room book")]
        public int NumberRoomBook { get; set; }
        [Display(Name = "Number Of Member")]
        [Required(ErrorMessage = "Need to input Number of member ")]
        public int NumberOfMember { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        
        public string UserId { get; set; }

        public float Price { get; set; }










    }
}