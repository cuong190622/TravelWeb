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

        
        public ActionResult ShowBooked() 
        {


            
            using (var abc = new EF.HotelContext())
            {
                String id = System.Security.Principal.WindowsIdentity.GetCurrent().GetUserId();
                var booked = abc.Bookings.Where(a => a.UserId == id ).ToList();
                return View(booked);
            }
        }

       

        public ActionResult ShowLocation()
        {

            using (var locations = new EF.HotelContext())
            {
                var Location = locations.Locations.OrderBy(a => a.Id).ToList();
                return View(Location);
            }
        }

        public ActionResult HotelLocation(int id)
        {

            using (var hotels = new EF.HotelContext())
            {
                //var hotel = from location in hotels.Locations
                //            join holtelLocation in hotels.Hotels on location.Id equals holtelLocation.LocationId select
                //            new hotelLocationviewmodal()
                //            {
                //                id = holtelLocation.Id,

                //                namehotel = holtelLocation.Name,
                //                star = holtelLocation.Star,
                //                description = holtelLocation.Star


                //             }

                var hotelloca = from hotel in hotels.Hotels where hotel.LocationId == id select hotel;
                return View(hotelloca.ToList());
            }
        }


        //public ActionResult ViewRooms(int id)
        //{

        //    using (var hotels = new EF.HotelContext())
        //    {
        //        //var hotel = from location in hotels.Locations
        //        //            join holtelLocation in hotels.Hotels on location.Id equals holtelLocation.LocationId select
        //        //            new hotelLocationviewmodal()
        //        //            {
        //        //                id = holtelLocation.Id,

        //        //                namehotel = holtelLocation.Name,
        //        //                star = holtelLocation.Star,
        //        //                description = holtelLocation.Star


        //        //             }

        //        var roomsht = from room in hotels.Rooms where room.HotelId == id select room;
        //        return View(roomsht.ToList());
        //    }
        //}



        private List<SelectListItem> getList(int id)
        {
          
                var stx = (from type in context.TypeRooms
                          where type.HotelId == id select new
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






        public ActionResult ShowTypeRoom(int id)
        {

          


                var Type = (from ty in context.TypeRooms
                            join bk in context.Bookings on ty.Id equals bk.TypeRoomId
                            select new BookingTypeDTO
                            {
                                Id = ty.Id,
                                NameType = ty.Name,
                                Image = ty.Image,
                                Price = ty.Price,
                                MaxNumberRoom = ty.MaxNumberRoom,
                                Description = ty.Description,
                                NumberRoomBook = bk.numberRoomBook,
                                HotelId = ty.HotelId,
                            }).ToList().Where(p => p.HotelId == id);
                // var Type = Db.TypeRooms.OrderBy(a => a.Id).ToList();
                return View(Type);
            

        }


























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
                using (var Database = new EF.HotelContext())
                {
                    a.CustomerName = a.CustomerName ;
                    
                    a.CustomerAdress = a.CustomerAdress;
                    a.CustomerPhone = a.CustomerPhone;
                    a.BookingFrom = a.BookingFrom;
                    a.BookingTo = a.BookingTo;
                    a.numberRoomBook = a.numberRoomBook;
                    a.NumberOfMember = a.NumberOfMember;
                    a.UserId = user.Id;
                    a.status = "wating";
                    Database.Bookings.Add(a);
                    Database.SaveChanges();
                   // await SendEmail((a.), "New user booking <p>Comment: " + a.RoomId + " </p>");


                }
            }
            TempData["message"] = $"Create new idea Successfully!";
            return RedirectToAction("Index");
        }

        private void Customvalidation(Booking a)
        {
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
           
                var free = BookingTypeDTO.a.RoomFree
            if (a.numberRoomBook > )
            {
                ModelState.AddModelError("BookingTo", "Please input to > from");
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