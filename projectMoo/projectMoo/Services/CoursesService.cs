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
                    Title = c.Title,
                    Description = c.Description,
                    Assignments = null
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
    }
}