using projectMoo.Models;
using projectMoo.Models.Entities;
using projectMoo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Services
{
    public class CourseService
    {
        private IAppDataContext _db;

        public CourseService(IAppDataContext context)
        {
            _db = context ?? new ApplicationDbContext();
        }


        /// <summary>
        /// Returns all existing courses
        /// </summary>
        /// <returns>List<Course></returns>
        public List<Course> GetAllCourses()
        {
            List<Course> courses = new List<Course>();

            courses = (from course in _db.Courses
                       select course).ToList();

            return courses;
        }

        /// <summary>
        /// Adds a user to a specific course
        /// </summary>
        /// <param name="userID">UserID</param>
        /// <param name="courseID">CourseID</param>
        public void AddUserToCourse(string userID , int courseID)
        {
            _db.UserCourses.Add(new UserCourse() { UserID = userID, CourseID = courseID });
        }


        /// <summary>
        /// Saves changes to the database
        /// </summary>
        public void SaveToDataBase()
        {
            _db.SaveChanges();
        }


        /// <summary>
        /// Add a group of users to a specific course
        /// </summary>
        /// <param name="groupName">Groupname</param>
        /// <param name="courseID">CourseID</param>
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


        /// <summary>
        /// Returns existing courses for a specific user
        /// </summary>
        /// <param name="userId">UserID</param>
        /// <returns>List<CourseViewModel></returns>
        public List<CourseViewModel> GetCoursesForUser(string userId)
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


        /// <summary>
        /// Get a course by its ID
        /// </summary>
        /// <param name="ID">CourseID</param>
        /// <returns>CourseViewModel</returns>
        public CourseViewModel GetCourseByID(int ID)
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


        /// <summary>
        /// Writes a course to the database
        /// </summary>
        /// <param name="c">Course</param>
        public void AddNewCourse(Course c)
        {
            _db.Courses.Add(c);
            _db.SaveChanges();
        }

      
        /// <summary>
        /// Delete a course from the database by its name
        /// </summary>
        /// <param name="courseName">CourseName</param>
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