using Microsoft.AspNet.Identity;
using projectMoo.Models.ViewModels;
using projectMoo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projectMoo.Controllers
{
    public class HomeController : Controller
    {
        private UserService _userService = new UserService();
        private AssignmentsService _assignmentService = new AssignmentsService();
        private CoursesService _courseService = new CoursesService();

        [Authorize]
        public ActionResult Index()
        {
            UserViewModel model = new UserViewModel();
            model.Name = _userService.getUserName(User.Identity.GetUserId());
            model.Assignments = _assignmentService.GetAssignmentForUser(User.Identity.GetUserId());
            model.Courses = _courseService.getCoursesForUser(User.Identity.GetUserId());

            /*
            var model = _assignmentService.GetAssignmentForUser(User.Identity.GetUserId());
            if(model == null)
            {
                System.Diagnostics.Debug.WriteLine("Index model is null");
            }
            */
            return View(model);
        }
    }
}