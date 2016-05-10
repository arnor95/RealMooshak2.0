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

                if (userCourse == null)
                    continue;

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

      
        public void DeleteCourseWithName(string courseName)
        {

            var courseToDelete = (from course in _db.Courses
                                 where course.Title == courseName
                                 select course).ToList();

            if (courseToDelete.Count == 0)
                return;


            int courseId = courseToDelete.First().ID;

            foreach (var connection in courseToDelete)
            {
                _db.Courses.Remove(connection);

            }



            var userCourses = (from course in _db.UserCourses
                                  where course.CourseID == courseId
                                  select course).ToList();



            foreach (var connection in userCourses)
            {
                _db.UserCourses.Remove(connection);

            }


            var assignments = (from assignemt in _db.Assignments
                               where assignemt.CourseID == courseId
                               select assignemt).ToList();

            if (assignments.Count == 0)
                return;

            foreach (var connection in assignments)
            {
                var milestones = (from milestone in _db.AssignmentMilestones
                                   where milestone.AssignmentID == connection.ID
                                   select milestone).ToList();

               

                foreach (var milestoneConnection in milestones)
                {
                    var submissions = (from submission in _db.Submissions
                                      where submission.MilestoneID == milestoneConnection.ID
                                      select submission).ToList();

                    foreach(var submissionConnection in submissions)
                    {
                        _db.Submissions.Remove(submissionConnection);
                    }

                    _db.AssignmentMilestones.Remove(milestoneConnection);
                }

                _db.Assignments.Remove(connection);

            }



        }
    }
}