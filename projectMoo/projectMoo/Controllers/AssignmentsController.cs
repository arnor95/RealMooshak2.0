using Microsoft.AspNet.Identity;
using projectMoo.Models;
using projectMoo.Models.Entities;
using projectMoo.Models.ViewModels;
using projectMoo.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projectMoo.Controllers
{
    public class AssignmentsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private AssignmentsService _assignmentService = new AssignmentsService();
        private CoursesService _courseService = new CoursesService();
        private MilestoneService _milestoneService = new MilestoneService();
        private SubmissionService _submissionService = new SubmissionService();

        [Authorize]
        // GET: Assignments
        public ActionResult CourseAssignments(int ID)
        {
            System.Diagnostics.Debug.WriteLine("Index Assign");
            UserViewModel model = new UserViewModel();
            model.Assignments = _assignmentService.GetAssignmentsInCourse(ID);
            model.Courses = _courseService.getCoursesForUser(User.Identity.GetUserId());
            model.Name = model.Courses.FirstOrDefault().Title;
            return View(model);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet]
        public ActionResult CreateAssignment()
        {
            List<Course> courses = new List<Course>();
            courses = _courseService.getAllCourses();

            List<SelectListItem> listItems = new List<SelectListItem>();

            foreach (Course c in courses)
            {
                listItems.Add(new SelectListItem
                {
                    Text = c.Title,
                    Value = c.ID.ToString()
                });
            }

            ViewData["Courses"] = listItems;

            AssignmentViewModel model = new AssignmentViewModel();
            model.Milestones = new List<AssignmentMilestoneViewModel>();
            var milestoneVM = new AssignmentMilestoneViewModel();
            milestoneVM.Input = new List<string>();
            milestoneVM.Input.Add("");
            milestoneVM.Output = new List<string>();
            milestoneVM.Output.Add("");
            model.Milestones.Add(milestoneVM);
           
            return View(model);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public ActionResult CreateAssignment(AssignmentViewModel data)
        {
            if (ModelState.IsValid)
            {

                System.Diagnostics.Debug.WriteLine(data);

                throw new Exception();

                Assignment assignment = new Assignment();
                assignment.CourseID = data.CourseID;
                assignment.Title = data.Title;
                assignment.Description = data.Description;
                assignment.DueDate = data.DueDate;
                _assignmentService.addNewAssignment(assignment);

                return RedirectToAction("AssignmentCreated");
            }

         


            return View(data);
        }

        public ActionResult AddMilestone()
        {
            var milestoneVM = new AssignmentMilestoneViewModel();
            milestoneVM.Input = new List<string>();
            milestoneVM.Input.Add("");
            milestoneVM.Output = new List<string>();
            milestoneVM.Output.Add("");
            return PartialView("~/Views/Shared/EditorTemplates/AssignmentMilestoneViewModel.cshtml", milestoneVM);
        }

        public ActionResult AssignmentCreated()
        {
            Success success = new Success();
            success.Title = "Success";
            success.Description = @"A news assignment was creted.";
            success.ActionTitle = "Create another assignment";
            success.ActionPath = @"CreateAssignment";

            return View("~/Views/Success/Success.cshtml", success);
        }

        public ActionResult DeleteAssignment()
        {
            List<SelectListItem> assignments = new List<SelectListItem>();

            var allAssignments = _assignmentService.GetAllAssignments();

            foreach (Assignment s in allAssignments)
            {
                assignments.Add(new SelectListItem
                {
                    Text = s.Title,
                    Value = s.Title

                });
            }

            ViewData["Assignments"] = assignments;

            return View(new DeleteAssignment());
        }


        public ActionResult UploadMilestone()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadMilestone(HttpPostedFileBase file, int ID)
        {
            string extension = Path.GetExtension(file.FileName);

            if (extension != ".cpp")
            {
                return View("Error");
            }

            if (file != null && file.ContentLength > 0)
            {
                string userID = User.Identity.GetUserId();
                Guid fileID = Guid.NewGuid();
                var fileName = fileID + extension;

                Submission newSubmission = new Submission();
                newSubmission.MilestoneID = ID;
                newSubmission.UserID = userID;
                newSubmission.FileID = fileName;

                _db.Submissions.Add(newSubmission);
                _db.SaveChanges();
                    
               
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/Code/"), fileName);
                file.SaveAs(path);
            }

            return View("Result");
        }

        public ActionResult ViewSubmissions(int ID)
        {
            SubmissionViewModel model = new SubmissionViewModel();

            model.Milestone = _milestoneService.getMilestoneByID(ID);
            model.Submissions = _submissionService.getAllSubmissionsByMilestoneID(ID);

            return View(model);
        }

    
    }
}