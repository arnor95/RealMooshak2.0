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
    public class AssignmentsController : Controller
    {
        private AssignmentsService _assignmentService = new AssignmentsService();
        private CoursesService _courseService = new CoursesService();

        [Authorize]
        // GET: Assignments
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("Index Assign");
            UserViewModel model = new UserViewModel();
            model.Assignments = _assignmentService.GetAssignmentForUser(User.Identity.GetUserId());
            model.Courses = _courseService.getCoursesForUser(User.Identity.GetUserId());
            return View(model);
        }
    }
}