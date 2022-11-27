using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
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
    public class LoginController : Controller
    {

        // logout
        public ActionResult LogOut() // function to Logout
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie); //Signout authentication cookie  
                ViewData.Clear(); // Clear All ViewData
                Session.RemoveAll(); // Clear All Session
            }
            return RedirectToAction("LogIn", "Login"); // Redirect user to login page
        }





            

        // GET: Login
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LogIn(UserInfo user)
        {
            CustomValidation(user);

            if (!ModelState.IsValid)
            {
                return View(user);
            }
            else
            {
                var context = new HotelContext();
                var store = new UserStore<UserInfo>(context);
                var manager = new UserManager<UserInfo>(store);

                var signInManager
                    = new SignInManager<UserInfo, string>(manager, HttpContext.GetOwinContext().Authentication);

                var fuser = await manager.FindByEmailAsync(user.Email);

                var result = await signInManager.PasswordSignInAsync(userName: user.Email.Split('@')[0], password: user.PasswordHash, isPersistent: false, shouldLockout: false);

                if (result == SignInStatus.Success)
                {
                    var userStore = new UserStore<UserInfo>(context);
                    var userManager = new UserManager<UserInfo>(userStore);


                    if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.Admin))
                    {
                        /*SessionLogin(fuser.UserName);*/
                        TempData["acb"] = fuser.UserName;
                        return RedirectToAction("Index", "Admin");
                    }
                    if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.User))
                    {
                        TempData["UserId"] = fuser.Id;
                        return RedirectToAction("ShowLocation", "User");
                    }

            
                    if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.Manager))
                    {
                        TempData["xyz"] = fuser.Id;
                        return RedirectToAction("ShowTypeRoom", "Managerht");
                    }
                    else return Content($"Comming Soon!!!");
                }
                else
                {
                    ModelState.AddModelError("PasswordHash", "User Name or Password incorrect!");
                    return View();
                }
            }

        }

        private void CustomValidation(UserInfo user)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                ModelState.AddModelError("Email", "Please input Email");
            }
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                ModelState.AddModelError("PasswordHash", "Please input PassWord");
            }
            if (!string.IsNullOrEmpty(user.Email) && (user.Email.Split('@')[0] == null) && (user.Email.Split('@')[1] == null) && (user.Email.Split('@')[1] != "gmail.com"))
            {
                ModelState.AddModelError("Email", "Please a valid Email (abc@gmail.com)");
            }
           
            if (!string.IsNullOrEmpty(user.PasswordHash) && user.PasswordHash.Length <= 7)
            {
                ModelState.AddModelError("PasswordHash", "PassWord need more than 7 characters");
            }
        }

        public async Task<ActionResult> CreateAdmin()
        {
            var context = new HotelContext();
            var store = new UserStore<UserInfo>(context);
            var manager = new UserManager<UserInfo>(store);

            var email = "cuong@gmail.com";
            var password = "cuong2001";
            var phone = "0961119526";
            var role = "admin";

            var user = await manager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new UserInfo
                {
                    UserName = email.Split('@')[0],
                    Email = email,
                    PhoneNumber = phone,
                    Name = email.Split('@')[0],
                    Role = role,
                };
                await manager.CreateAsync(user, password);
                await CreateRole(user.Email, "admin");
                return Content($"Create Admin account Succsess");
            }
            return RedirectToAction("LogIn");
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

            if (!await roleManager.RoleExistsAsync(SecurityRoles.User))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = SecurityRoles.User });
            }
            if (!await roleManager.RoleExistsAsync(SecurityRoles.Manager))
            {

                await roleManager.CreateAsync(new IdentityRole { Name = SecurityRoles.Manager });

            }
            
            var User = await userManager.FindByEmailAsync(email); // gán role cho user (thêm role ) 

            if (!await userManager.IsInRoleAsync(User.Id, SecurityRoles.Admin) && role == "admin")
            {
                userManager.AddToRole(User.Id, SecurityRoles.Admin);
            }
            if (!await userManager.IsInRoleAsync(User.Id, SecurityRoles.User) && role == "user")
            {
                userManager.AddToRole(User.Id, SecurityRoles.User);
            }
            if (!await userManager.IsInRoleAsync(User.Id, SecurityRoles.Manager) && role == "manager")
            {
                userManager.AddToRole(User.Id, SecurityRoles.Manager);
            }
            
            return Content("done!");
        }
    }
}