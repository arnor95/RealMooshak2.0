using projectMoo.Models;
using projectMoo.Models.Entities;
using projectMoo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Services
{
    public class CoursesService
    {
        private ApplicationDbContext _db;

        public CoursesService()
        {
            _db = new ApplicationDbContext();
        }

        public List<Course> getAllCourses()
        {
            List<Course> courses = new List<Course>();

            courses = (from course in _db.Courses
                       select course).ToList();

            return courses;
        }

        public void AddUserToCourse(string userID , int courseID)
        {
            _db.UserCourses.Add(new UserCourse() { UserID = userID, CourseID = courseID });
        }

        public void SaveToDataBase()
        {
            _db.SaveChanges();
        }

        public void AddUsersBasedOnGroup(string groupName, int courseID)
        {
            List<UserGroup> usersInGroup = (from userGroup in _db.UserGroups
                                           where  userGroup.GroupName == groupName
                                           select userGroup).ToList();

            foreach (UserGroup g in usersInGroup)
            {
                AddUserToCourse(g.UserID, courseID);
            }

        }

        public List<CourseViewModel> getCoursesForUser(string userId)
        {
            var links = (from courseRelation in _db.UserCourses
                               where courseRelation.UserID == userId
                               select courseRelation).ToList();

            List<Course> courses = new List<Course>();

            List<CourseViewModel> coursesViewModel = new List<CourseViewModel>();


            foreach ( UserCourse c in links)
            {
                Course userCourse = (from storedCourse in _db.Courses
                              where storedCourse.ID == c.CourseID
                              select storedCourse).SingleOrDefault();

                courses.Add(userCourse);

            }

            foreach (Course c in courses)
            {
                coursesViewModel.Add(new CourseViewModel
                {
                    CourseID = c.ID,
                    Title = c.Title,
                    Description = c.Description,
                    Assignments = null,
                    BeginDate = c.BeginDate,
                    EndDate = c.EndDate
                });
            }

            return coursesViewModel;
        }

        public CourseViewModel getCourseByID(int ID)
        {
            var course = (from c in _db.Courses
                          where c.ID == ID
                          select c).SingleOrDefault();

            if (course == null)
            {
                System.Diagnostics.Debug.WriteLine("No course found with that ID");
            }

            CourseViewModel returnCourse = new CourseViewModel
            {
                Title = course.Title,
                Description = course.Description,
                Assignments = new List<Assignment>()
            };

            return returnCourse;
        }

        public void addNewCourse(Course c)
        {
            _db.Courses.Add(c);
            _db.SaveChanges();
        }
    }
}