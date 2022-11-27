using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelWeb.Models
{
    public class Room
    {
        public int Id { get; set; }
        
        public string RoomNo { get; set; }
        public string Status { get; set; }

        public int TypeId { get; set; }
         
    }
}