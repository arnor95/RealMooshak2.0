using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projectMoo.Models.Entities;
using projectMoo.Models.ViewModels;
using projectMoo.Services;

namespace projectMoo.Controllers
{
    public class TeacherController : Controller
    {
        SubmissionService _submissionService = new SubmissionService();

        // GET: Teacher
        public ActionResult AllAssignmentsForMilestone(int ID)
        {
            List<SubmissionsForTeacherViewModel> model = _submissionService.getSubmissionsForTeacherByMilestoneID(ID);

            return View(model);
        }
    }
}