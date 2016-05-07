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
            string[] systemGroups = new[] {"None" ,"1st year students","2nd year students", "3rd year students" };

            List<UserRole> students = new List<UserRole>();
            List<UserRole> teachers = new List<UserRole>();

            foreach (var user in allusers)
            {
                if (manager.IsInRole(user.Id, "Student"))
                {
                    students.Add(new UserRole() { Username = user.UserName, Roles = "Student", UserId = user.Id, Selected = false });

                }
                else if (manager.IsInRole(user.Id, "Teacher"))
                {
                    teachers.Add(new UserRole() { Username = user.UserName, Roles = "Teacher" , UserId = user.Id, Selected = false});

                }
            }
            AddCourseViewModel model = new AddCourseViewModel() { Students = students, Teachers = teachers, course = new Course(), Group = "None"};
            List<SelectListItem> groups = new List<SelectListItem>();
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateCourse(AddCourseViewModel data)
        {
            if (ModelState.IsValid)
            {
                Course c = new Course();
                c = data.course;
                _courseService.addNewCourse(c);

                if(data.Group == "None" && data.Group != null)
                {
                    //TODO save this course for all people in the selected group
                }

                foreach(UserRole user in data.Students)
                {
                    if (user.Selected)
                    {
                        _courseService.AddUserToCourse(user.UserId, c.ID);
                    }
                }

                foreach (UserRole user in data.Teachers)
                {
                    if (user.Selected)
                    {
                        _courseService.AddUserToCourse(user.UserId, c.ID);
                    }
                }

                _courseService.SaveToDataBase();


                //TODO: connect the selected teachers/students to the course

                return RedirectToAction("Index");
            }

            return View(data);
        }

        public ActionResult DeleteCourse()
        {
            return View(new DeleteCourseViewModel());
        }

        [HttpPost]
        public ActionResult DeleteCourse(DeleteCourseViewModel model)
        {
            return RedirectToAction("Index");
        }
    }
}