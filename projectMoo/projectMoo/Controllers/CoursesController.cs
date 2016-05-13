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

        private CoursesService _courseService = new CoursesService(null);
        private ApplicationUserManager manager;

        [Authorize]
        // GET: Courses
        public ActionResult Index()
        {
            //throw new Exception();

            string currentUserId = User.Identity.GetUserId();
            System.Diagnostics.Debug.WriteLine("user id " + currentUserId);
            List<CourseViewModel> ViewModel = _courseService.GetCoursesForUser(currentUserId);
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
                else if (manager.IsInRole(user.Id, "Teacher") || manager.IsInRole(user.Id, "Admin"))
                {
                    teachers.Add(new UserRole() { Username = user.UserName, Roles = "Teacher" , UserId = user.Id, Selected = false});

                }
            }
            AddCourseViewModel model = new AddCourseViewModel() { Students = students, Teachers = teachers, course = new Course()};
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
        public ActionResult CreateCourse(AddCourseViewModel addCourseViewModel)
        {
            if (ModelState.IsValid)
            {
               
                _courseService.AddNewCourse(addCourseViewModel.course);

                if(!(addCourseViewModel.course.Group == "None") && addCourseViewModel.course.Group != null)
                {
                    //TODO save this course for all people in the selected group
                    _courseService.AddUsersBasedOnGroup(addCourseViewModel.course.Group, addCourseViewModel.course.ID);
                    _courseService.SaveToDataBase();

                    return RedirectToAction("Index");

                }

                foreach (UserRole user in addCourseViewModel.Students)
                {
                    if (user.Selected)
                    {
                        _courseService.AddUserToCourse(user.UserId, addCourseViewModel.course.ID);
                    }
                }

                foreach (UserRole user in addCourseViewModel.Teachers)
                {
                    if (user.Selected)
                    {
                        _courseService.AddUserToCourse(user.UserId, addCourseViewModel.course.ID);
                    }
                }

                _courseService.SaveToDataBase();



                return RedirectToAction("Index");
            }

            return View(addCourseViewModel);
        }

        public ActionResult DeleteCourse()
        {

            List<SelectListItem> courses = new List<SelectListItem>();

            var allCourses = _courseService.GetAllCourses();

            foreach (Course s in allCourses)
            {
                courses.Add(new SelectListItem
                {
                    Text = s.Title,
                    Value = s.Title

                });
            }

            ViewData["Courses"] = courses;

            return View(new DeleteCourseViewModel());
        }

        [HttpPost]
        public ActionResult DeleteCourse(DeleteCourseViewModel model)
        {
            if(model.courseName != null)
            {
                _courseService.DeleteCourseWithName(model.courseName);
                _courseService.SaveToDataBase();

                Success success = new Success();
                success.Title = "Success";
                success.Description = @"Course deleted";
                success.ActionTitle = "Delete another user";
                success.ActionPath = @"DeleteCourse";

                return View("~/Views/Success/Success.cshtml", success);
            }
            else
            {
                return RedirectToAction("Index");

            }

        }
    }
}