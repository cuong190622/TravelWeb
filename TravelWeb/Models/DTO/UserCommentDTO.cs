using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelWeb.Models.DTO
{
    public class UserCommentDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Comment { get; set; }

        public string TimeComment { get; set; }
    }
}