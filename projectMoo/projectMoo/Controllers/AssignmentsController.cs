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

            return View(new Assignment());
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public ActionResult CreateAssignment(Assignment a)
        {
            if (ModelState.IsValid)
            {
                Assignment assignment = new Assignment();
                UpdateModel(assignment);
                _assignmentService.addNewAssignment(assignment);

                return RedirectToAction("Index");
            }

            return View(a);
        }

        public ActionResult DeleteAssignment()
        {
            return View(new DeleteAssignment());
        }

        public ActionResult DeleteCourse(DeleteCourseViewModel model)
        {
            return RedirectToAction("Index");
        }

        public ActionResult UploadMilestone()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadMilestone(HttpPostedFileBase file, int ID)
        {
            if (file != null && file.ContentLength > 0)
            {
                string userID = User.Identity.GetUserId();
                Guid fileID = Guid.NewGuid();
                var fileName = fileID + Path.GetExtension(file.FileName);

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
    }
}