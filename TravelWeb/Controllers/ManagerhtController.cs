using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TravelWeb.EF;
using TravelWeb.Models;
using TravelWeb.Models.DTO;

namespace TravelWeb.Controllers
{
    public class ManagerhtController : Controller
    {
        private readonly HotelContext context;


        // open db context 
        public ManagerhtController()
        {
            context = new HotelContext();
        }
 
        // GET: Managerht
        public ActionResult Index()
        {
            return View();  
        }


        //////////////////////////////
        [HttpGet]
        public ActionResult CreateTypeRoom()
        {
            ViewBag.Hotel = getListHt();
            return View();
        }

        [HttpPost]
        public ActionResult CreateTypeRoom(TypeRoom a)
        {

            CustomValidationLocation(a);
            if (!ModelState.IsValid)
            {

                ViewBag.Hotel = getListHt();
                return View(a); // return lai Create.cshtml

            }
            else
            {

                using (var loca = new EF.HotelContext())
                {
                    if (a.Description == null)
                    {
                        a.Description = "No Description!";
                    }
                    loca.TypeRooms.Add(a);
                    loca.SaveChanges();
                }

                //TempData["message"] = $"Successfully add Location {a.Name} to system!";

                return RedirectToAction("Index");
            }



        }

        private void CustomValidationLocation(TypeRoom a)
        {
           
            if (string.IsNullOrEmpty(a.Image))
            {
                ModelState.AddModelError("Image", "Please input ImageUrl");
            }
           
            
        }

        [HttpGet]
        public ActionResult EditTypeRoom(int id)
        {
            ViewBag.Hotel = getListHt();
            // lay category qua id tu db
            using (var Loca = new EF.HotelContext())
            {
                var Location = Loca.TypeRooms.FirstOrDefault(c => c.Id == id);
                return View(Location);
            }
        }

        [HttpPost]
        public ActionResult EditTypeRoom(int id, TypeRoom a)
        {
            CustomValidationLocation(a);
            if (!ModelState.IsValid)
            {
                ViewBag.Hotel = getListHt();
                return View(a); // return lai Create.cshtml
                                //di kem voi data ma user da go vao
            }
            else
            {
                using (var cate = new EF.HotelContext())
                {
                    cate.Entry<TypeRoom>(a).State = System.Data.Entity.EntityState.Modified;

                    cate.SaveChanges();
                }

                return RedirectToAction("Index");
            }

        }

        public ActionResult ShowTypeRoom()
        {

                var Type = (from ty in context.TypeRooms
                            join ht in context.Hotels on ty.HotelId equals ht.Id into hoteltype
                            from ht in hoteltype.DefaultIfEmpty()
                            select new TypeRoomDTO                           
                            {
                                TypeId = ty.Id,
                                NameType = ty.Name,
                                Image = ty.Image,
                                Price = ty.Price,
                                MaxNumberRoom = ty.MaxNumberRoom,
                                Description = ty.Description,
                                HotelName = ht.Name,
                            }).AsEnumerable();

                // var Type = Db.TypeRooms.OrderBy(a => a.Id).ToList();
                return View(Type);

        }









        public ActionResult ShowWating()
        {
           var request = (from bk in context.Bookings 
                          join ty in context.TypeRooms on bk.TypeRoomId equals ty.Id 
                          join ht in context.Hotels on ty.HotelId equals ht.Id select
                          new BookingTypeHtDTO
                          {
                              Id = bk.id,
                              CustomerName = bk.CustomerName,
                              CustomerAdress = bk.CustomerAdress,
                              CustomerPhone = bk.CustomerPhone,
                              BookingFrom = bk.BookingFrom,
                              BookingTo = bk.BookingTo,
                              NumberOfMember = bk.NumberOfMember,
                              NumberRoomBook = bk.numberRoomBook,
                              Types = ty.Name,
                              Statuses =bk.status,
                              HotelName = ht.Name,
                              Price = ty.Price,


                          }
                         ).ToList().Where(p => p.Statuses == "wating");
            


                //var request = (from re in context.Bookings
                //              where re.status == "wating"
                //              select re).ToList();
                return View(request);
            

        }

        //private List<SelectListItem> getList()
        //{
        //    using (var abc = new EF.HotelContext())
        //    {
        //        var stx = 
        //        // var bookinghotel = from room in abc.Rooms where room.HotelId == id select room;
        //        //  return bookinghotel;
        //        return stx;
        //    }
        //}

        public ActionResult ShowActive()
        {
            var request = (from bk in context.Bookings
                           join ty in context.TypeRooms on bk.TypeRoomId equals ty.Id
                           join ht in context.Hotels on ty.HotelId equals ht.Id
                           select
                            new BookingTypeHtDTO
                            {
                            Id = bk.id,
                            CustomerName = bk.CustomerName,
                            CustomerAdress = bk.CustomerAdress,
                            CustomerPhone = bk.CustomerPhone,
                            BookingFrom = bk.BookingFrom,
                            BookingTo = bk.BookingTo,
                            NumberOfMember = bk.NumberOfMember,
                            NumberRoomBook = bk.numberRoomBook,
                            Types = ty.Name,
                            Statuses = bk.status,
                            HotelName = ht.Name,
                            Price = ty.Price,


                            }
                         ).ToList().Where(p => p.Statuses == "active");
            return View(request);
        }


        public ActionResult ShowHistory()
        {
            var request = (from bk in context.Bookings
                           join ty in context.TypeRooms on bk.TypeRoomId equals ty.Id
                           join ht in context.Hotels on ty.HotelId equals ht.Id
                           select
                            new BookingTypeHtDTO
                            {
                                Id = bk.id,
                                CustomerName = bk.CustomerName,
                                CustomerAdress = bk.CustomerAdress,
                                CustomerPhone = bk.CustomerPhone,
                                BookingFrom = bk.BookingFrom,
                                BookingTo = bk.BookingTo,
                                NumberOfMember = bk.NumberOfMember,
                                NumberRoomBook = bk.numberRoomBook,
                                Types = ty.Name,
                                Statuses = bk.status,
                                HotelName = ht.Name,
                                Price = ty.Price,


                            }
                         ).ToList().Where(p => p.Statuses == "history");
            return View(request);
        }





        [HttpGet]
        public ActionResult Confirm(int id)
        {
            // CustomValidationBooking(a);
            using(var abc = new EF.HotelContext())
            {
                var booked = abc.Bookings.FirstOrDefault(x => x.id == id);
                if (booked != null)
                {
                    booked.status = "active";
                    abc.SaveChanges();

                    return RedirectToAction("ShowWating");
                }
                return RedirectToAction("ShowWating");
            } 

        }
        [HttpGet]
        public ActionResult Checkout(int id)
        {
            // CustomValidationBooking(a);
            using (var abc = new EF.HotelContext())
            {
                var booked = abc.Bookings.FirstOrDefault(x => x.id == id);
                if (booked != null)
                {

                    //abc.Bookings.Remove(booked);
                    //abc.SaveChanges();
                    booked.status = "history";
                    abc.SaveChanges();
                    return RedirectToAction("ShowWating");
                }
                return RedirectToAction("ShowWating");
            }

        }

        //private void CustomValidationBooking(Booking booked)
        //{
        //    throw new NotImplementedException();
        //}

     

        [HttpGet]
        public ActionResult DeleteTypeRoom(int id, TypeRoom a)
        {
            using (var classes = new EF.HotelContext())
            {
                var Class = classes.TypeRooms.FirstOrDefault(c => c.Id == id);
                return View(Class);
            }
        }

        [HttpPost]
        public ActionResult DeleteTypeRoom(int id)
        {
            using (var abc = new EF.HotelContext())
            {
                var xxx = abc.TypeRooms.FirstOrDefault(b => b.Id == id);
                if (xxx != null)
                {
                    abc.TypeRooms.Remove(xxx);
                    abc.SaveChanges();
                }
                TempData["message"] = $"Successfully delete with Id: {xxx.Id}";
                return RedirectToAction("Index");
            }
        }

        ////////////////////////////////////////////// 
        // Room
       
        private List<SelectListItem> getListHt()
        {
            using (var ht = new EF.HotelContext())
            {
                var htc = ht.Hotels.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList();
                return htc;
            }
        }

        

    }
}