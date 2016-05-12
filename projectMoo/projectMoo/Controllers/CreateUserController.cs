using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using projectMoo.Models;
using projectMoo.Models.Entities;
using projectMoo.Models.ViewModels;
using projectMoo.Services;
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
        private ApplicationUserManager manager;
        private UserService userService = new UserService(null);

        // GET: CreateUser
        public ActionResult Index()
        {
            


            return View();
        }

        public ActionResult UserCreated()
        {
            Success success = new Success();
            success.Title = "Success";
            success.Description = @"A new user was creted.";
            success.ActionTitle = "Create another user";
            success.ActionPath = @"NewUser";

            return View("~/Views/Success/Success.cshtml",success);
        }

        public ActionResult UserDeleted()
        {
            Success success = new Success();
            success.Title = "Success";
            success.Description = @"The user has been deleted.";
            success.ActionTitle = "Delete another user";
            success.ActionPath = @"DeleteUser";

            return View("~/Views/Success/Success.cshtml", success);
        }


        [HttpGet]
        public ActionResult NewUser()
        {
            NewUserViewModel model = new NewUserViewModel();

            List<SelectListItem> roles = new List<SelectListItem>();
            List<SelectListItem> groups = new List<SelectListItem>();

            string[] systemRoles = new[] {"None", "Admin", "Teacher", "Student" };
            string[] systemGroups = new[] {"None", "1st year students", "2nd year students", "3rd year students" };

            foreach (string s in systemRoles)
            {
                roles.Add(new SelectListItem
                {
                    Text = s,
                    Value = s
                
                });
            }

            ViewData["Roles"] = roles;


            foreach (string s in systemGroups)
            {
                groups.Add(new SelectListItem
                {
                    Text = s,
                    Value = s

                });
            }

            ViewData["Groups"] = groups;

            return View(model);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> NewUser(NewUserViewModel data)
        {
            if (ModelState.IsValid)
            {
                manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                
                var user = new ApplicationUser { UserName = data.Email, Email = data.Email };
                var result = await manager.CreateAsync(user, data.Password);

                if (result.Succeeded)
                { 
                    var roleresult = manager.AddToRole(user.Id, data.Role);
                    userService.AddUserToGroup(user.Id, data.Group);
                    userService.AddInfoForUser(data.Name, data.Phone, user.Id);
                    userService.SaveToDatabase();
                    return RedirectToAction("UserCreated");

                }
                else
                {
                    //whoops error;
                    return View("Error");
                }

            }
            else
            {
                List<SelectListItem> roles = new List<SelectListItem>();
                List<SelectListItem> groups = new List<SelectListItem>();

                string[] systemRoles = new[] { "None", "Admin", "Teacher", "Student" };
                string[] systemGroups = new[] { "None", "1st year students", "2nd year students", "3rd year students" };

                foreach (string s in systemRoles)
                {
                    roles.Add(new SelectListItem
                    {
                        Text = s,
                        Value = s

                    });
                }

                ViewData["Roles"] = roles;


                foreach (string s in systemGroups)
                {
                    groups.Add(new SelectListItem
                    {
                        Text = s,
                        Value = s

                    });
                }

                ViewData["Groups"] = groups;
                return View(data);

            }

        }

        public ActionResult DeleteUser()
        {
            return View(new DeleteUserViewModel());
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> DeleteUser(DeleteUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.userEmail != null)
                {
                    manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var user = await manager.FindByEmailAsync(model.userEmail);

                    userService.DeleteConnectionsToUser(user.Id);

                    var rolesForUser = await manager.GetRolesAsync(user.Id);

                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var roleName in rolesForUser.ToList())
                        {
                            var result = await manager.RemoveFromRoleAsync(user.Id, roleName);
                        }
                    }

                    await manager.DeleteAsync(user);
                    return RedirectToAction("UserDeleted");

                }
            }

            return View("Error");

        }

    }

   
}