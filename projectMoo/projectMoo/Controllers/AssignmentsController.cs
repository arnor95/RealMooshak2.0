﻿using Microsoft.AspNet.Identity;
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
        private AssignmentService _assignmentService = new AssignmentService(null);
        private CourseService _courseService = new CourseService(null);
        private MilestoneService _milestoneService = new MilestoneService(null);
        private SubmissionService _submissionService = new SubmissionService(null);

        #region Display Assignments
        [Authorize]
        // GET: Assignments
        public ActionResult CourseAssignments(int ID , int AssignmentID)
        {
            CourseAssignmentsViewModel model = new CourseAssignmentsViewModel();
            model.Assignments = _assignmentService.GetAssignmentsInCourse(ID);
            model.Courses = _courseService.GetCoursesForUser(User.Identity.GetUserId());
            model.Course = _courseService.GetCourseByID(ID);
            model.Name = model.Course.Title;
            model.Active = AssignmentID;
            return View(model);
        }

        #endregion

        #region Create Assignment
        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet]
        public ActionResult CreateAssignment()
        {
            List<Course> courses = new List<Course>();
            courses = _courseService.GetAllCourses();

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

            return View(model);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public ActionResult CreateAssignment(AssignmentViewModel data)
        {
            if (ModelState.IsValid)
            {

                System.Diagnostics.Debug.WriteLine(data);

                Assignment assignment = new Assignment();
                assignment.CourseID = data.CourseID;
                assignment.Title = data.Title;
                assignment.Description = data.Description;
                assignment.DueDate = data.DueDate;
                _assignmentService.AddNewAssignment(assignment);
                _assignmentService.SaveToDatabase();

                _milestoneService.AddMilestonesForAssignment(assignment.ID, data.Milestones);
                _milestoneService.SaveToDatabase();
             
                return RedirectToAction("AssignmentCreated");
            }
            else
            {
                List<Course> courses = new List<Course>();
                courses = _courseService.GetAllCourses();

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

                return View(data);
            }
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

        #endregion

        #region Delete Assignment

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

        [HttpPost]
        public ActionResult DeleteAssignment(DeleteAssignment model)
        {
            if (model.assignmentName != null)
            {
                _assignmentService.DeleteAssignmentWithName(model.assignmentName);
                _assignmentService.SaveToDatabase();

                Success success = new Success();
                success.Title = "Success";
                success.Description = @"Assignment deleted";
                success.ActionTitle = "Delete another assignment";
                success.ActionPath = @"DeleteAssignment";

                return View("~/Views/Success/Success.cshtml", success);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region Handin Code
        public ActionResult UploadMilestone()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadMilestone(HttpPostedFileBase file, int ID)
        {
            string extension = Path.GetExtension(file.FileName);
            ResultViewModel model = new ResultViewModel();


            if (extension != ".cpp")
            {
                return View("Error");
            }

            if (file != null && file.ContentLength > 0)
            {
                string userID = User.Identity.GetUserId();
                Guid fileID = Guid.NewGuid();
                var fileName = fileID + extension;

                string realFileID = fileID.ToString();

                string newFolderPath = Server.MapPath("~/Code/" + userID + "/" + ID);
                Directory.CreateDirectory(newFolderPath);
                
                var path = Path.Combine(Server.MapPath("~/Code/" + userID + "/" + ID + "/"), fileName);
                file.SaveAs(path);

                model = _submissionService.CompileCode(newFolderPath, realFileID, ID);

                Submission newSubmission = new Submission();
                newSubmission.MilestoneID = ID;
                newSubmission.UserID = userID;
                newSubmission.FileID = path;
                newSubmission.Date = DateTime.Now;
                newSubmission.State = model.Status;

                if (model.Status)
                {
                    MilestoneFinished m = new MilestoneFinished
                    {
                        MilestoneID = ID,
                        UserID = User.Identity.GetUserId()
                    };

                    _db.MilestoneFinisheds.Add(m);
                    _db.SaveChanges();
                }

                _db.Submissions.Add(newSubmission);
                _db.SaveChanges();
            }

            return View("Result", model);
        }

        #endregion

        #region Submissions
        public ActionResult ViewSubmissions(int ID)
        {
            SubmissionViewModel model = new SubmissionViewModel();

            model.Milestone = _milestoneService.GetMilestoneByID(ID);
            model.Submissions = _submissionService.GetAllSubmissionsByMilestoneIDForUser(ID);

            return View(model);
        }

        public ActionResult Download(string file)
        {
            if (!System.IO.File.Exists(file))
            {
                return HttpNotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(file);
            var response = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = "code.cpp"
            };
            return response;
        }

        #endregion

    }
}