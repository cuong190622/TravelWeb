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

namespace TravelWeb.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (var ct = new EF.HotelContext())
            {
                var user = ct.Users.Where(a => a.Role != "admin").OrderBy(a => a.Id).ToList();
                return View(user);
            }
        }


        [HttpGet]
        public ActionResult CreateLocation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateLocation(Location a)
        {

            CustomValidationLocation(a);
            if (!ModelState.IsValid)
            {
                
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
                    loca.Locations.Add(a);
                    loca.SaveChanges();
                }
              

                return RedirectToAction("ShowLocation");
            }



        }


        [HttpGet]
        public ActionResult EditLocation(int id)
        {
            // lay category qua id tu db
            using (var Loca = new EF.HotelContext())
            {
                var Location = Loca.Locations.FirstOrDefault(c => c.Id == id);
                return View(Location);
            }
        }

        [HttpPost]
        public ActionResult EditLocation(int id, Location a)
        {
            CustomValidationLocation(a);
            if (!ModelState.IsValid)
            {
                return View(a); // return lai Create.cshtml
                                //di kem voi data ma user da go vao
            }
            else
            {
                using (var cate = new EF.HotelContext())
                {
                    cate.Entry<Location>(a).State = System.Data.Entity.EntityState.Modified;

                    cate.SaveChanges();
                }

                return RedirectToAction("ShowLocation");
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

        [HttpGet]
        public ActionResult DeleteLocation(int id, Location a)
        {
            using (var classes = new EF.HotelContext())
            {
                var Class = classes.Locations.FirstOrDefault(c => c.Id == id);
                return View(Class);
            }
        }

        [HttpPost]
        public ActionResult DeleteLocation(int id)
        {
            using (var abc = new EF.HotelContext())
            {
                var xxx = abc.Locations.FirstOrDefault(b => b.Id == id);
                if (xxx != null)
                {
                    abc.Locations.Remove(xxx);
                    abc.SaveChanges();
                }
                TempData["message"] = $"Successfully delete with Id: {xxx.Id}";
                return RedirectToAction("ShowLocation");
            }
        }
        ////// <Manager Hotel> 
        ///  

        private List<SelectListItem> getList()
        {
            using (var abc = new EF.HotelContext())
            {
                var stx = abc.Locations.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList();
                return stx;
            }
        }

        [HttpGet]
        public ActionResult CreateHotel()
        {
            ViewBag.Class = getList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateHotel(Hotel a)
        {

            CustomValidationHotel(a);
            if (!ModelState.IsValid)
            {

                ViewBag.Class = getList();
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
                    loca.Hotels.Add(a);
                    loca.SaveChanges();
                }
            

                return RedirectToAction("ShowHotel");
            }



        }

        

        [HttpGet]
        public ActionResult EditHotel(int id)
        {
            // lay category qua id tu db
            ViewBag.Class = getList();
            
            using (var Loca = new EF.HotelContext())
            {
                var hotel = Loca.Hotels.FirstOrDefault(c => c.Id == id);
                return View(hotel);
            }
        }

        [HttpPost]
        public ActionResult EditHotel(int id, Hotel a)
        {
            CustomValidationHotel(a);
            if (!ModelState.IsValid)
            {
                ViewBag.Class = getList();
                return View(a); // return lai Create.cshtml
                                //di kem voi data ma user da go vao
            }
            else
            {
                using (var hote = new EF.HotelContext())
                {
                    hote.Entry<Hotel>(a).State = System.Data.Entity.EntityState.Modified;

                    hote.SaveChanges();
                }

                return RedirectToAction("ShowHotel");
            }

        }

        private void CustomValidationHotel(Hotel a)
        {
            if (string.IsNullOrEmpty(a.Description))
            {
                ModelState.AddModelError("Description", "Please input Description");
            }
            if (string.IsNullOrEmpty(a.Name))
            {
                ModelState.AddModelError("Name", "Please input Name");
            }
            if (string.IsNullOrEmpty(a.HotelImage))
            {
                ModelState.AddModelError("Image", "Please input Image");
            }
        }
        [Authorize(Roles = SecurityRoles.Admin)]
        public ActionResult ShowHotel()
        {

            using (var hote = new EF.HotelContext())
            {
                var Hote = hote.Hotels.OrderBy(a => a.Id).ToList();
                return View(Hote);
            }
        }

        [HttpGet]
        public ActionResult DeleteHotel(int id, Hotel a)
        {
            using (var classes = new EF.HotelContext())
            {
                var Class = classes.Hotels.FirstOrDefault(c => c.Id == id);
                return View(Class);
            }
        }

        [HttpPost]
        public ActionResult DeleteHotel(int id)
        {
            using (var abc = new EF.HotelContext())
            {
                var xxx = abc.Hotels.FirstOrDefault(b => b.Id == id);
                if (xxx != null)
                {
                    abc.Hotels.Remove(xxx);
                    abc.SaveChanges();
                }
                TempData["message"] = $"Successfully delete with Id: {xxx.Id}";
                return RedirectToAction("ShowHotel");
            }
        }

        /// <ManagerAccount >
        /// /////////////////////////////////////////////////////////////////////////////

        public ActionResult CreateUser()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(UserInfo User)
        {

            CustomValidationUser(User);

            if (!ModelState.IsValid)
            {
                
                
                return View(User);
            }
            else
            {
                var context = new HotelContext();
                var store = new UserStore<UserInfo>(context);
                var manager = new UserManager<UserInfo>(store);

                var cus = await manager.FindByEmailAsync(User.Email);

                if (cus == null)
                {
                    cus = new UserInfo
                    {
                        UserName = User.Email.Split('@')[0],
                        Email = User.Email,
                        Age = User.Age,
                        Phone = User.Phone,
                        DoB = User.DoB,
                        
                        Role = "user",
                        PhoneNumber = User.PhoneNumber,
                        PasswordHash = "cuong123",
                        Name = User.Name
                    };
                    await manager.CreateAsync(cus, cus.PasswordHash);
                    await CreateRole(User.Email, "user");
                    TempData["message"] = $"Account successfully created!";
                }
                else
                {
                    TempData["alert"] = $"Email already exists, please try again!!";
                }
                return RedirectToAction("Index");
            }


        }



        // Edit user account 

        [HttpGet]
        public ActionResult EditAccount(string id)
        {
            HotelContext context = new HotelContext();
            var roleManager = new Microsoft.AspNet.Identity.RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new Microsoft.AspNet.Identity.UserManager<UserInfo>(new UserStore<UserInfo>(context));
            using (var bwCtx = new HotelContext())
            {
               
                var ct = bwCtx.Users.FirstOrDefault(t => t.Id == id);
                if (ct != null)
                {
                    return View(ct);
                }
                else
                {
                    return RedirectToAction("Index"); //redirect to action in the same controller
                }
            }
        } 

        [HttpPost]
        public async Task<ActionResult> EditAccount(string id, UserInfo newUser)
        {
            CustomValidationUser(newUser);

            if (!ModelState.IsValid)
            {
                return View(newUser);
            }
            else
            {
                var context = new HotelContext();
                var store = new UserStore<UserInfo>(context);
                var manager = new UserManager<UserInfo>(store);

                var user = await manager.FindByEmailAsync(newUser.Email);

                if (user != null)
                {
                    user.UserName = newUser.Email.Split('@')[0];
                    user.Email = newUser.Email;
                    user.PasswordHash = "cuong123";
                    user.Name = newUser.Name;
                    user.Phone = newUser.Phone;
                    user.Age = newUser.Age;
                    user.DoB = newUser.DoB;
                    await manager.UpdateAsync(user);
                }
                return RedirectToAction("Index");
            }
           
        }



        [HttpGet]
        public ActionResult DeleteUser(string id)
        {
            using (var FAPCtx = new EF.HotelContext())
            {
                var user = FAPCtx.Users.FirstOrDefault(c => c.Id == id);

                if (user != null)
                {
                   
                    return View(user);
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
        }

      
        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id, UserInfo user)
        {
            var context = new HotelContext();
            var store = new UserStore<UserInfo>(context);
            var manager = new UserManager<UserInfo>(store);

            var User = await manager.FindByIdAsync(id);

            if (User != null)
            {
                await manager.DeleteAsync(User);
            }

            return RedirectToAction("Index");

        }






        // Managerr account

        public ActionResult CreateManager()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateManager(UserInfo Manager)
        {

            CustomValidationUser(Manager);

            if (!ModelState.IsValid)
            {
             
                return View(Manager);
            }
            else
            {
                var context = new HotelContext();
                var store = new UserStore<UserInfo>(context);
                var manager = new UserManager<UserInfo>(store);

                var cus = await manager.FindByEmailAsync(Manager.Email);

                if (cus == null)
                {
                    cus = new UserInfo
                    {
                        UserName = Manager.Email.Split('@')[0],
                        Email = Manager.Email,
                        Age = Manager.Age,
                        Phone = Manager.Phone,
                        DoB = Manager.DoB,

                        Role = "manager",
                        PhoneNumber = Manager.PhoneNumber,
                        PasswordHash = "cuong123",
                        Name = Manager.Name
                    };
                    await manager.CreateAsync(cus, cus.PasswordHash);
                    await CreateRole(Manager.Email, "manager");
                    TempData["message"] = $"Account successfully created!";
                }
                else
                {
                    TempData["alert"] = $"Email already exists, please try again!!";
                }
                return RedirectToAction("Index");
            }


        }



        // Edit Manager account 

        [HttpGet]
        public ActionResult EditManager(string id)
        {
            


            
            using (var htCtx = new HotelContext())
            {

                var ct = htCtx.Users.FirstOrDefault(t => t.Id == id);
                if (ct != null)
                {
                    return View(ct);
                }
                else
                {
                    return RedirectToAction("Index"); //redirect to action in the same controller
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditManager(string id, UserInfo newUser)
        {
            CustomValidationUser(newUser);

            if (!ModelState.IsValid)
            {
                return View(newUser);
            }
            else
            {
                var context = new HotelContext();
                var store = new UserStore<UserInfo>(context);
                var manager = new UserManager<UserInfo>(store);

                var user = await manager.FindByEmailAsync(newUser.Email);

                if (user != null)
                {
                    user.UserName = newUser.UserName;
                    user.Email = newUser.Email;
                    user.PasswordHash = "cuong123";
                    user.Name = newUser.Name;
                    user.Phone = newUser.Phone;
                    user.Age = newUser.Age;
                    user.DoB = newUser.DoB;
                    await manager.UpdateAsync(user);
                }
                return RedirectToAction("Index");
            }

        }



        [HttpGet]
        public ActionResult DeleteManager(string id)
        {
            using (var htCtx = new EF.HotelContext())
            {
                var user = htCtx.Users.FirstOrDefault(c => c.Id == id);

                if (user != null)
                {

                    return View(user);
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
        }


        [HttpPost]
        public async Task<ActionResult> DeleteManager(string id, UserInfo user)
        {
            var context = new HotelContext();
            var store = new UserStore<UserInfo>(context);
            var manager = new UserManager<UserInfo>(store);

            var User = await manager.FindByIdAsync(id);

            if (User != null)
            {
                await manager.DeleteAsync(User);
            }

            return RedirectToAction("Index");

        }










        public async Task<ActionResult> CreateRole(string email, string role) // tạo role cho identity 
        {
            var context = new HotelContext();
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var userStore = new UserStore<UserInfo>(context);
            var userManager = new UserManager<UserInfo>(userStore);

            if (!await roleManager.RoleExistsAsync(SecurityRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = SecurityRoles.Admin });
            }

            if (!await roleManager.RoleExistsAsync(SecurityRoles.Manager))
            {

                await roleManager.CreateAsync(new IdentityRole { Name = SecurityRoles.Manager });

            }
            if (!await roleManager.RoleExistsAsync(SecurityRoles.User))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = SecurityRoles.User });

            }

            var User = await userManager.FindByEmailAsync(email); // gán role cho user (thêm role ) 

            if (!await userManager.IsInRoleAsync(User.Id, SecurityRoles.Admin) && role == "admin")
            {
                userManager.AddToRole(User.Id, SecurityRoles.Admin);
            }
            if (!await userManager.IsInRoleAsync(User.Id, SecurityRoles.Manager) && role == "manager")
            {
                userManager.AddToRole(User.Id, SecurityRoles.Manager);
            }
            if (!await userManager.IsInRoleAsync(User.Id, SecurityRoles.User) && role == "user")
            {
                userManager.AddToRole(User.Id, SecurityRoles.User);
            }
            return Content("done!");
        }



        public void CustomValidationUser(UserInfo User)
        {
            if (string.IsNullOrEmpty(User.Email))
            {
                ModelState.AddModelError("Email", "Please input Email");
            }
        }






        private void CustomValidationLocation(Location a)
        {
            if (string.IsNullOrEmpty(a.Description))
            {
                ModelState.AddModelError("Description", "Please input Description");
            }
            if (string.IsNullOrEmpty(a.Name))
            {
                ModelState.AddModelError("Name", "Please input Name");
            }
        }









    }



}