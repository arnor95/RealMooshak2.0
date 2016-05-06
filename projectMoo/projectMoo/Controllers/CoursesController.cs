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

namespace projectMoo.Controllers
{
    public class CoursesController : Controller
    {

        private CoursesService _courseService = new CoursesService();
        private ApplicationUserManager manager;

        [Authorize]
        // GET: Courses
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            System.Diagnostics.Debug.WriteLine("user id " + currentUserId);
            List<CourseViewModel> ViewModel = _courseService.getCoursesForUser(currentUserId);
            return View(ViewModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateCourse()
        {

        ApplicationDbContext context = new ApplicationDbContext();

            var allusers = context.Users.ToList();
            manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            List<UserRole> students = new List<UserRole>();
            List<UserRole> teachers = new List<UserRole>();

            foreach (var user in allusers)
            {
                if (manager.IsInRole(user.Id, "Student"))
                {
                    students.Add(new UserRole { Username = user.UserName, Roles = "Student" });

                }
                else if (manager.IsInRole(user.Id, "Teacher"))
                {
                    teachers.Add(new UserRole { Username = user.UserName, Roles = "Teacher" });

                }
            }
            AddCourseViewModel model = new AddCourseViewModel { Students = students, Teachers = teachers, course = new Course()};

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateCourse(AddCourseViewModel data)
        {
            if (ModelState.IsValid)
            {
                Course c = new Course();
                c = data.course;
                _courseService.addNewCourse(c);

                return RedirectToAction("Index");
            }

            return View(data);
        }
    }
}