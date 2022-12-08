using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TravelWeb.EF;
using TravelWeb.Models;
using TravelWeb.Models.DTO;


namespace TravelWeb.Controllers
{
    public class UserController : Controller
    {

        private readonly HotelContext context;


        // open db context 
        public UserController()
        {
            context = new HotelContext();
        }
        // GET: user
        public ActionResult Index()
        {
            

            return View();
        }

        [Authorize(Roles = SecurityRoles.User)]
        public async Task<ActionResult> ShowBooked() 
        {

            var context = new HotelContext();
            var store = new UserStore<UserInfo>(context);
            var manager = new UserManager<UserInfo>(store);
            var user = await manager.FindByIdAsync(User.Identity.GetUserId());
            

            using (var abc = new EF.HotelContext())
            {
               

               
                var booked = abc.Bookings.Where(a => a.UserId == user.Id ).ToList();
                return View(booked);
            }
        }


        [Authorize(Roles = SecurityRoles.User)]
        public ActionResult ShowLocation()
        {
          
                var Location = context.Locations.OrderBy(a => a.Id).ToList();
                return View(Location);
            
        }
        [Authorize(Roles = SecurityRoles.User)]
        public ActionResult HotelLocation(int id)
        {

            using (var hotels = new EF.HotelContext())
            {
               

                var hotelloca = from hotel in hotels.Hotels where hotel.LocationId == id select hotel;
                return View(hotelloca.ToList());
            }
        }



        





        private List<SelectListItem> getList(int id)
        {
          
                var stx = (from type in context.TypeRooms
                          where type.Id == id select new
                          {
                              type.Name,
                              type.Id   
                          }).ToList()
                          .Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList();
               // var bookinghotel = from room in abc.Rooms where room.HotelId == id select room;
              //  return bookinghotel;
                return stx;
            
        }





        [Authorize(Roles = SecurityRoles.User)]
        public ActionResult ShowTypeRoom(int id)
        {


            var Type = (from ty in context.TypeRooms
                        join ht in context.Hotels on ty.HotelId equals ht.Id into hoteltype
                        from ht in hoteltype.DefaultIfEmpty()
                        select new TypeRoomDTO
                        {
                            TypeId = ty.Id,
                            HotelId = ht.Id,
                            NameType = ty.Name,
                            Image = ty.Image,
                            Price = ty.Price,
                            MaxNumberRoom = context.Rooms.Where(x => x.TypeId == ty.Id).Count(),
                            Description = ty.Description,
                            HotelName = ht.Name,
                            AvailableRoom = ty.AvailableRoom != null ? ty.AvailableRoom : context.Rooms.Where(x => x.TypeId == ty.Id && x.Status.Equals("available")).Count(),
                        }).Where(p => p.HotelId == id).ToList();



            
            return View(Type);
                

        }




        // Create Comment

        private List<SelectListItem> getListType(int id)
        {

            var stx = (from type in context.TypeRooms
                       where type.Id == id
                       select new
                       {
                           type.Name,
                           type.Id
                       }).ToList()
                      .Select(p => new SelectListItem
                      {
                          Text = p.Name,
                          Value = p.Id.ToString()
                      }).ToList();
            // var bookinghotel = from room in abc.Rooms where room.HotelId == id select room;
            //  return bookinghotel;
            return stx;

        }





        [HttpGet]
        public ActionResult CreateComment(int id)
        {
            ViewBag.comment = getListType(id);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateComment(Comment a)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ShowComment", new { TypeId = a.TypeId });
            }

            var context = new HotelContext();
            var store = new UserStore<UserInfo>(context);
            var manager = new UserManager<UserInfo>(store);
            var user = await manager.FindByIdAsync(User.Identity.GetUserId());

            a.Date = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            a.CommentType = a.CommentType;
            a.UserId = user.Id;
            context.Comments.Add(a);
            context.SaveChanges();
            return RedirectToAction("ShowComment", new { TypeId = a.TypeId });
        }



        // Show Comment
        [Authorize(Roles = SecurityRoles.User)]
        public ActionResult ShowComment()
        {


            var Comment = (from cm in context.Comments
                           join us in context.Users on cm.UserId equals us.Id into usercomment
                           from uc in usercomment.DefaultIfEmpty()
                           select new UserCommentDTO
                           {
                               Id = cm.Id,
                               UserName = uc.UserName,
                               Comment = cm.CommentType,
                               TimeComment = cm.Date

                           }).ToList();


            return View(Comment);


        }









        [Authorize(Roles = SecurityRoles.User)]
        [HttpGet]
        public ActionResult CreateBooking(int id)
        {
           
                ViewBag.Booking = getList(id);
                return View();
           
        }

       


        [HttpPost]
        public async Task<ActionResult> CreateBooking(Booking a, FormCollection f,int id)
        {
           
            Customvalidation(a);
            if (!ModelState.IsValid)//if user input wrong
            {

                //SetViewBag();
                ViewBag.Booking = getList(id);
                return View(a);
            }
            else
            {
                var context = new HotelContext();
                var store = new UserStore<UserInfo>(context);
                var manager = new UserManager<UserInfo>(store);
                var user = await manager.FindByIdAsync(User.Identity.GetUserId());
                    
                    var typeRoom = context.TypeRooms.FirstOrDefault(x => x.Id == a.TypeRoomId);

                    a.Price = typeRoom.Price * a.NumberRoomBook;  
                    a.UserId = user.Id;
                    a.Status = "wating";

                    context.Bookings.Add(a);
                    // await SendEmail((a.), "New user booking <p>Comment: " + a.RoomId + " </p>");
                    
                    typeRoom.AvailableRoom = context.Rooms.Where(x => x.TypeId == a.TypeRoomId && x.Status.Equals("available")).Count() - a.NumberRoomBook;
                    context.SaveChanges();



            }
            TempData["message"] = $"Create new idea Successfully!";
            return RedirectToAction("ShowLocation");
        }

        private void Customvalidation(Booking a)
        {
            var RoomFreebk = context.TypeRooms.FirstOrDefault(x => x.Id == a.TypeRoomId).AvailableRoom;
            if (RoomFreebk == null)
            {
                RoomFreebk = context.Rooms.Where(x => x.TypeId == a.TypeRoomId && x.Status.Equals("available")).Count();
            }
            
            
            if (string.IsNullOrEmpty(a.CustomerName))
            {
                ModelState.AddModelError("CustomerName", "Please input Name");
            }
            if (a.BookingFrom <= DateTime.Now )
            {
                ModelState.AddModelError("BookingFrom", "Please input from > now");
            }
            if (a.BookingTo <= DateTime.Now  )
            {
                ModelState.AddModelError("BookingTo", "Please input to > now");
            }
            
                if (a.BookingFrom >= DateTime.Now && a.BookingTo >= DateTime.Now && a.BookingTo <= a.BookingFrom)
            {
                ModelState.AddModelError("BookingTo", "Please input to > from");
            }
                if(RoomFreebk  < a.NumberRoomBook )
            {
                ModelState.AddModelError("numberRoomBook", "Please input numberRoomBook < RoomFree");

            }
           
               
        }

        public async Task SendEmail(string email, string booked)
        {
            var body = "<p>Email From: {0} </p><p>Message: {1}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress("dinhcuong22062001@gmail.com");
            message.Subject = "New email from BookingHotel.vn";
            message.Body = string.Format(body, "Admin", booked);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "dinhcuong22062001@gmail.com",
                    Password = "cuong2001"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }


      



    }
}