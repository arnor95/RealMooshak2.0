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
        private AssignmentsService _service = new AssignmentsService();

        [Authorize]
        // GET: Assignments
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("Index Assign");
            var ViewModel = _service.GetAssignmentsInCourse(1);
            return View(ViewModel);
        }
    }
}