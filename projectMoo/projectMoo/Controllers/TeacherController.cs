using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projectMoo.Models.Entities;
using projectMoo.Services;

namespace projectMoo.Controllers
{
    public class TeacherController : Controller
    {
        SubmissionService _submissionService = new SubmissionService();

        // GET: Teacher
        public ActionResult AllAssignmentsForMilestone(int ID)
        {
            List<Submission> model = _submissionService.getAllSubmissionsByMilestoneID(ID);

            return View(model);
        }
    }
}