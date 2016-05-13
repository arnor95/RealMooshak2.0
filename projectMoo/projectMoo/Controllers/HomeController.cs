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
        private UserService _userService = new UserService(null);
        private AssignmentService _assignmentService = new AssignmentService(null);
        private CourseService _courseService = new CourseService(null);

        #region My Page
        [Authorize]
        public ActionResult Index()
        {
            UserViewModel model = new UserViewModel();
            string userID = User.Identity.GetUserId();
            UserInfo info = _userService.GetInfoForUser(userID);


            if (info.Name == null)
            {
                model.Name = "Please add a name";
            }
            else
            {
                model.Name = info.Name;
            }
            
            model.Assignments = _assignmentService.GetAssignmentForUser(userID);
            model.Courses = _courseService.GetCoursesForUser(userID);

            if (info.Phone == null)
            {
                model.Phone = "Please enter a phone number";
            }
            else
            {
                model.Phone = info.Phone;
            }
            
            var picID = info.PicID;

            if (picID != null)
            {
                model.PicID = picID;
            }
            else
            {
                model.PicID = "profile.png";
            }
            
            return View(model);
        }

        #endregion

        #region Profile Picture
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
                var path = Path.Combine(Server.MapPath("~/Images/Profile/"), fileName);
                file.SaveAs(path);
            }
            return RedirectToAction("Index");
        }
        #endregion

    }
}