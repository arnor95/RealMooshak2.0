using Microsoft.AspNet.Identity;
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
            var students = allusers.Where(x => x.Roles.Select(role => role.RoleId).Equals("2")).ToList();
            System.Diagnostics.Debug.WriteLine("students " + students);

            var userVM = students.Select(user => new UserRole { Username = user.Email, Roles = string.Join(",", user.Roles.Select(role => role.RoleId)) }).ToList();

            var teachers = allusers.Where(x => x.Roles.Select(role => role.RoleId).Equals("3")).ToList();
            var adminsVM = teachers.Select(user => new UserRole { Username = user.Email, Roles = string.Join(",", user.Roles.Select(role => role.RoleId)) }).ToList();

            AddCourseViewModel model = new AddCourseViewModel { Students = userVM, Teachers = adminsVM, course = new Course()};

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateCourse(Course data)
        {
            if (ModelState.IsValid)
            {
                Course c = new Course();
                UpdateModel(c);
                _courseService.addNewCourse(c);

                return RedirectToAction("Index");
            }

            return View(data);
        }
    }
}