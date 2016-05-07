﻿using Microsoft.AspNet.Identity;
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
        private UserService userService = new UserService();

        // GET: CreateUser
        public ActionResult Index()
        {
            


            return View();
        }

        public ActionResult UserCreated()
        {
            Success success = new Success();
            success.Title = "Success";
            success.Description = @"A news user was creted.";
            success.ActionTitle = "Create another user";
            success.ActionPath = @"NewUser";

            return View("~/Views/Success/Success.cshtml",success);
        }

        [HttpGet]
        public ActionResult NewUser()
        {
            NewUserViewModel model = new NewUserViewModel();

            List<SelectListItem> roles = new List<SelectListItem>();
            List<SelectListItem> groups = new List<SelectListItem>();

            string[] systemRoles = new[] { "Admin", @"Teacher", @"Student" };
            string[] systemGroups = new[] {"None", "1st year students", @"2nd year students", @"3rd year students" };

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
                    return RedirectToAction("UserCreated");

                }
                else
                {
                    //whoops error;
                }

            }

            return View(data);
        }

        public ActionResult DeleteUser()
        {
            return View(new DeleteUserViewModel());
        }

        [HttpPost]
        public ActionResult DeleteUser(DeleteUserViewModel model)
        {
            return RedirectToAction("Index");
        }

    }

   
}