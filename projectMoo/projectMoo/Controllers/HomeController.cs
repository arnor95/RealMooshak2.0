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
    public class HomeController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private UserService _userService = new UserService();
        private AssignmentsService _assignmentService = new AssignmentsService();
        private CoursesService _courseService = new CoursesService();

        [Authorize]
        public ActionResult Index()
        {
            UserViewModel model = new UserViewModel();
            string userID = User.Identity.GetUserId();

            model.Name = _userService.getUserName(userID);
            model.Assignments = _assignmentService.GetAssignmentForUser(userID);
            model.Courses = _courseService.getCoursesForUser(userID);
            model.Phone = _userService.getUserPhone(userID);

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