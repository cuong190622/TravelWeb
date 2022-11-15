using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TravelWeb.Models;

namespace TravelWeb.EF
{
    public class HotelContext : IdentityDbContext<UserInfo>
    {
        public HotelContext() : base("TvConnection")
        {

        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<TypeRoom> TypeRooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }



    }
}