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
            var picID = _userService.getUserPic(userID);

            if (picID != null)
            {
                model.PicID = picID;

                SessionCourse.Instance.SetActiveCourse(model.Courses.FirstOrDefault());

                return View(model);
            }
            
            model.PicID = "profile.png";

            SessionCourse.Instance.SetActiveCourse(model.Courses.FirstOrDefault());
            

            /*
            var model = _assignmentService.GetAssignmentForUser(User.Identity.GetUserId());
            if(model == null)
            {
                System.Diagnostics.Debug.WriteLine("Index model is null");
            }
            */
            return View(model);
        }

        public ActionResult UploadPic()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadPic(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                var fileExtension = Path.GetExtension(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/Images/Profile/"), User.Identity.GetUserId() + fileExtension);
                file.SaveAs(path);
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
        }
    }
}