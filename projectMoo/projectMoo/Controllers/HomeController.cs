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
            UserInfo info = _userService.getInfoForUser(userID);

            model.Name = info.Name;
            model.Assignments = _assignmentService.GetAssignmentForUser(userID);
            model.Courses = _courseService.getCoursesForUser(userID);
            model.Phone =info.Phone;
            var picID = info.PicID;

            if (picID != null)
            {
                model.PicID = picID;
            }
            else
            {
                model.PicID = "profile.png";
            }

            //SessionCourse.Instance.SetActiveCourse(model.Courses.FirstOrDefault());
            
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
                string userID = User.Identity.GetUserId();

                var fileName = userID + Path.GetExtension(file.FileName);
                var updateUser = (from user in _db.UserInfoes
                                  where user.UserID == userID
                                  select user).SingleOrDefault();

                

                updateUser.PicID = fileName;
                _db.SaveChanges();
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/Images/Profile/"), fileName);
                file.SaveAs(path);

               
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
        }
    }
}