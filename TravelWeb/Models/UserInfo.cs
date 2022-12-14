using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TravelWeb.Models
{
    public class UserInfo : IdentityUser
    {
        public string Role { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }

        [DataType(DataType.Date)]
        public string DoB { get; set; }
      
    }
}