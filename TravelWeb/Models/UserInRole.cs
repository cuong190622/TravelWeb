﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelWeb.Models
{
    public class UserInRole
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Education { get; set; }
        public string Phone { get; set; }
        public string WorkingPlace { get; set; }
        public string Type { get; set; }

        public int bookingId { get; set; }
        public List<Booking> listRoomsBookinig { get; set; }
    }
}