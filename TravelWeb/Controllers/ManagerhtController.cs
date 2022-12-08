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
        [Authorize(Roles = SecurityRoles.Manager)]
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

               
                    if (a.Description == null)
                    {
                        a.Description = "No Description!";
                    }

                    context.TypeRooms.Add(a);
                    context.SaveChanges();
                

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
        [Authorize(Roles = SecurityRoles.Manager)]
        [HttpGet]
        public ActionResult EditTypeRoom(int id)
        {
            ViewBag.Hotel = getListHt();
            // lay category qua id tu db
           
                var Location = context.TypeRooms.FirstOrDefault(c => c.Id == id);
                return View(Location);
            
        }

        [HttpPost]
        public ActionResult EditTypeRoom(int id, TypeRoom a)
        {
            CustomValidationLocation(a);
            if (!ModelState.IsValid)
            {
                ViewBag.Hotel = getListHt();
                return View(a); 
            }
            else
            {
               
                    context.Entry<TypeRoom>(a).State = System.Data.Entity.EntityState.Modified;

                    context.SaveChanges();
                

                return RedirectToAction("Index");
            }

        }
        [Authorize(Roles = SecurityRoles.Manager)]
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
                            MaxNumberRoom = context.Rooms.Where(x => x.TypeId == ty.Id).Count(),
                            Description = ty.Description,
                            HotelName = ht.Name,
                            AvailableRoom = ty.AvailableRoom != null ? ty.AvailableRoom : context.Rooms.Where(x => x.TypeId == ty.Id && x.Status.Equals("available")).Count(),
                        }).AsEnumerable();

            // var Type = Db.TypeRooms.OrderBy(a => a.Id).ToList();
            return View(Type);

        }








        [Authorize(Roles = SecurityRoles.Manager)]
        public ActionResult ShowWating()
        {
            var request = (from bk in context.Bookings
                           join ty in context.TypeRooms on bk.TypeRoomId equals ty.Id
                           join ht in context.Hotels on ty.HotelId equals ht.Id
                           select
                                new BookingTypeHtDTO
                                {
                                    Id = bk.Id,
                                    CustomerName = bk.CustomerName,
                                    CustomerAdress = bk.CustomerAdress,
                                    CustomerPhone = bk.CustomerPhone,
                                    BookingFrom = bk.BookingFrom,
                                    BookingTo = bk.BookingTo,
                                    NumberOfMember = bk.NumberOfMember,
                                    NumberRoomBook = bk.NumberRoomBook,
                                    Types = ty.Name,
                                    Statuses = bk.Status,
                                    HotelName = ht.Name,
                                    Price = ty.Price,


                                }
                          ).ToList().Where(p => p.Statuses == "wating");



            //var request = (from re in context.Bookings
            //              where re.status == "wating"
            //              select re).ToList();
            return View(request);


        }


        public ActionResult ManagerRoomBooking()
        {
            var bookingroom = (from br in context.BookingRooms
                           join ro in context.Rooms on br.RoomId equals ro.Id
                           join bk in context.Bookings on br.BookingId equals bk.Id
                           join ty in context.TypeRooms on  ro.TypeId equals ty.Id
                           join ht in context.Hotels on ty.HotelId equals ht.Id
                           select
                                new BookingRoomDTO
                                {
                                    Id = br.Id,
                                    CustomerName = bk.CustomerName,
                                    CustomerAdress = bk.CustomerAdress,
                                    RoomNo = ro.RoomNo,
                                    Type = ty.Name
                                   


                                }
                          ).ToList();



            return View(bookingroom);


        }





        [Authorize(Roles = SecurityRoles.Manager)]
        public ActionResult ShowActive()
        {
            var request = (from bk in context.Bookings
                           join ty in context.TypeRooms on bk.TypeRoomId equals ty.Id
                           join ht in context.Hotels on ty.HotelId equals ht.Id
                           select
                            new BookingTypeHtDTO
                            {
                                Id = bk.Id,
                                CustomerName = bk.CustomerName,
                                CustomerAdress = bk.CustomerAdress,
                                CustomerPhone = bk.CustomerPhone,
                                BookingFrom = bk.BookingFrom,
                                BookingTo = bk.BookingTo,
                                NumberOfMember = bk.NumberOfMember,
                                NumberRoomBook = bk.NumberRoomBook,
                                Types = ty.Name,
                                Statuses = bk.Status,
                                HotelName = ht.Name,
                                Price = ty.Price,


                            }
                         ).ToList().Where(p => p.Statuses == "active");
            return View(request);
        }

        [Authorize(Roles = SecurityRoles.Manager)]
        public ActionResult ShowHistory()
        {
            var request = (from bk in context.Bookings
                           join ty in context.TypeRooms on bk.TypeRoomId equals ty.Id
                           join ht in context.Hotels on ty.HotelId equals ht.Id
                           select
                            new BookingTypeHtDTO
                            {
                                Id = bk.Id,
                                CustomerName = bk.CustomerName,
                                CustomerAdress = bk.CustomerAdress,
                                CustomerPhone = bk.CustomerPhone,
                                BookingFrom = bk.BookingFrom,
                                BookingTo = bk.BookingTo,
                                NumberOfMember = bk.NumberOfMember,
                                NumberRoomBook = bk.NumberRoomBook,
                                Types = ty.Name,
                                Statuses = bk.Status,
                                HotelName = ht.Name,
                                Price = ty.Price,


                            }
                         ).ToList().Where(p => p.Statuses == "history");
            return View(request);
        }


        public ActionResult ShowLate()
        {
            var request = (from bk in context.Bookings
                           join ty in context.TypeRooms on bk.TypeRoomId equals ty.Id
                           join ht in context.Hotels on ty.HotelId equals ht.Id
                           select
                            new BookingTypeHtDTO
                            {
                                Id = bk.Id,
                                CustomerName = bk.CustomerName,
                                CustomerAdress = bk.CustomerAdress,
                                CustomerPhone = bk.CustomerPhone,
                                BookingFrom = bk.BookingFrom,
                                BookingTo = bk.BookingTo,
                                NumberOfMember = bk.NumberOfMember,
                                NumberRoomBook = bk.NumberRoomBook,
                                Types = ty.Name,
                                Statuses = bk.Status,
                                HotelName = ht.Name,
                                Price = ty.Price,


                            }
                         ).ToList().Where(p => p.Statuses == "late");
            return View(request);
        }



        [HttpGet]
        public ActionResult Confirm(int id)
        {
            

            var booked = context.Bookings.FirstOrDefault(x => x.Id == id);
            var availableRooms = context.Rooms.Where(x => x.TypeId == booked.TypeRoomId && x.Status.Equals("available")).ToList();
            ViewBag.AvailableRooms = availableRooms;
            //if (booked != null)
            //{
            //    booked.Status = "active";
            //    context.SaveChanges();


            //}
            ViewBag.BookingId = id; 
            return View();


        }

        [HttpGet]
        public ActionResult Checkout(int id)
        {
            // CustomValidationBooking(a);
         
            
                var booked = context.Bookings.FirstOrDefault(x => x.Id == id);
                if (booked != null)
                {

                var roomcount = context.BookingRooms.Where(x => x.BookingId == id);
                foreach (var room in roomcount)
                {

                   
                    var RoomBook = context.Rooms.FirstOrDefault(x => x.Id == room.RoomId);
                    RoomBook.Status = "available";


                }


                var roomType = context.TypeRooms.FirstOrDefault(x => x.Id == booked.TypeRoomId);
                roomType.AvailableRoom += roomcount.Count();

                    
                   
                booked.Status = "history";
                    context.SaveChanges();
                    return RedirectToAction("ShowHistory");
                }
                return RedirectToAction("ShowWating");
            

        }


        [HttpGet]
        public ActionResult CheckinLate(int id)
        {
            // CustomValidationBooking(a);


            var booked = context.Bookings.FirstOrDefault(x => x.Id == id);
            if (booked != null)
            {

                var roomcount = context.BookingRooms.Where(x => x.BookingId == id);
                //foreach (var room in roomcount)
                //{


                //    var RoomBook = context.Rooms.FirstOrDefault(x => x.Id == room.RoomId);
                //    RoomBook.Status = "available";


                //}


                var roomType = context.TypeRooms.FirstOrDefault(x => x.Id == booked.TypeRoomId);
                roomType.AvailableRoom += booked.NumberRoomBook;



                booked.Status = "late";
                context.SaveChanges();
                return RedirectToAction("ShowWating");
            }
            return RedirectToAction("ShowWating");


        }



        [HttpPost]
        public ActionResult AddRoom([System.Web.Http.FromBody] ArrayDTO arr)
        {
            var booked = context.Bookings.FirstOrDefault(x => x.Id == arr.BookingId);
            booked.Status = "active";

            //CheckedRooms = arr.Array;
            foreach (var room in arr.Array)
            {

                var roomId = Convert.ToInt32(room);
                var bookingroom = new BookingRoom
                {
                    BookingId = booked.Id,
                    RoomId = roomId
                };
                context.BookingRooms.Add(bookingroom);
                var roomDb = context.Rooms.FirstOrDefault(x => x.Id == roomId);
                roomDb.Status = "not available";



                context.SaveChanges();
            }
            return RedirectToAction("ShowWating", "Managerht");

        }
















        [Authorize(Roles = SecurityRoles.Manager)]
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
           
            //if(context.Rooms.Where(x=> x.TypeId == id).Any())
            //{
            //    TempData[]
            //}
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


        private List<SelectListItem> getListtype(int id)
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

      
            return stx;

        }


        ///////
        ///

        [Authorize(Roles = SecurityRoles.Manager)]
        [HttpGet]
        public ActionResult CreateRoom(int id)
        {
            ViewBag.Type = getListtype(id);
            return View();
        }

        [HttpPost]
        public ActionResult CreateRoom(Room a, int id)
        {

            CustomValidationLocation(a);
            if (!ModelState.IsValid)
            {

                ViewBag.Type = getListtype(id);
                return View(a); // return lai Create.cshtml

            }
            else
            {

                a.Status = "available";
                context.Rooms.Add(a);
                context.SaveChanges();


                //TempData["message"] = $"Successfully add Location {a.Name} to system!";

                return RedirectToAction("Index");
            }
 
        }

        private void CustomValidationLocation(Room a)
        {
            if (string.IsNullOrEmpty(a.RoomNo))
            {
                ModelState.AddModelError("RoomNo", "Please input RoomNo");
            }
        }
    }
}