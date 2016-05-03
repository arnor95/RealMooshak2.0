using Microsoft.AspNet.Identity;
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
            return View(new Course());
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