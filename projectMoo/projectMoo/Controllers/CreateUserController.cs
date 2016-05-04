using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using projectMoo.Models;
using projectMoo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace projectMoo.Controllers
{
    public class CreateUserController : Controller
    {
        // GET: CreateUser
        public ActionResult Index()
        {
           
            return View();
        }

        [HttpGet]
        public ActionResult NewUser()
        {
            return View(new NewUserViewModel());
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> NewUser(NewUserViewModel data)
        {
            if (ModelState.IsValid)
            {
                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var user = new ApplicationUser { UserName = data.Email, Email = data.Email };
                var result = await manager.CreateAsync(user, data.Password);
                if (result.Succeeded)
                {
                    if(data.Role == "Admin" || data.Role == "Teacher"|| data.Role == "Student")
                    {
                        var roleresult = manager.AddToRole(user.Id, data.Role);
                    }

                    return RedirectToAction("Index");

                }
                else
                {
                    //whoops error;
                }

            }

            return View(data);
        }
    }
}