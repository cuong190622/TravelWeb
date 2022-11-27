using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TravelWeb.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Display(Name = "Comment")]
        public string CommentType { get; set; }

        public int TypeId { get; set; }
        public string UserId { get; set; }
        [DataType(DataType.Date)]
        public string Date { get; set; }
    }
}