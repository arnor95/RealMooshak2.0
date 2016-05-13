using Microsoft.AspNet.Identity;
using projectMoo.Models;
using projectMoo.Models.Entities;
using projectMoo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Services
{
    public class AssignmentsService
    {
        private IAppDataContext _db;
        private CoursesService _courseService;
        private MilestoneService _milestoneService;
        private UserService _userService;

        public AssignmentsService(IAppDataContext context)
        {
            _db = context ?? new ApplicationDbContext();
            _courseService = new CoursesService(null);
            _milestoneService = new MilestoneService(null);
            _userService = new UserService(null);
        }


        #region Get all assignments
        /// <summary>
        /// Returns all existing assignments in all courses
        /// </summary>
        /// <returns>List<Assignment></returns>
        public List<Assignment>  GetAllAssignments()
        {
            var assignments = (from assignment in _db.Assignments
                               select assignment).ToList();
            return assignments;
        }

        #endregion

        /// <summary>
        /// Returns all existing assignments for a user
        /// </summary>
        /// <param name="userID">UserID</param>
        /// <returns>List<AssignmentViewModel></returns>
        public List<AssignmentViewModel> GetAssignmentForUser(string userID)
        {
            List<AssignmentViewModel> returnList = new List<AssignmentViewModel>();

            var links = (from courseRelation in _db.UserCourses
                         where courseRelation.UserID == userID
                         select courseRelation).ToList();

            List<Course> courses = new List<Course>();

            foreach (UserCourse c in links)
            {
                Course userCourse = (from storedCourse in _db.Courses
                                     where storedCourse.ID == c.CourseID
                                     select storedCourse).SingleOrDefault();

                if (userCourse == null)
                    continue;

                courses.Add(userCourse);
            }

            foreach (Course item in courses)
            {
                returnList.AddRange(GetAssignmentsInCourse(item.ID));
            }

            return returnList;
        }


        /// <summary>
        /// Returns all existing assignment in a course
        /// </summary>
        /// <param name="CourseID">CourseID</param>
        /// <returns>List<AssignmentViewModel></returns>
        public List<AssignmentViewModel> GetAssignmentsInCourse(int CourseID)
        {
            var Assignments = (from assignment in _db.Assignments
                               where assignment.CourseID == CourseID
                               select assignment).ToList();



            if (Assignments == null)
            {
                System.Diagnostics.Debug.WriteLine("No assignment with that course ID found");
            }

            List<AssignmentViewModel> listAssignments = new List<AssignmentViewModel>();
            string userID = HttpContext.Current.User.Identity.GetUserId();



            foreach (var assign in Assignments)
            {
                System.Diagnostics.Debug.WriteLine(assign.CourseID);

                listAssignments.Add(new AssignmentViewModel
                {
                    Title = assign.Title,
                    CourseTitle = _courseService.GetCourseByID(assign.CourseID).Title.ToString(),
                    CourseID = CourseID,
                    Description = assign.Description,
                    Status = _userService.HasFinishedAssignment(userID, assign.ID),
                    Milestones = _milestoneService.GetMilestonesForAssignment(assign.ID),
                    DueDate = assign.DueDate,
                    AssignmentID = assign.ID
                    
                });
            }

            return listAssignments;
        }

        /// <summary>
        /// Returns a single assignment
        /// </summary>
        /// <param name="AssignmentID">AssignmentID</param>
        /// <returns>AssignmentViewModel</returns>
        public Assignment GetAssignmentByID(int AssignmentID)
        {
            var Assignment = _db.Assignments.SingleOrDefault(x => x.ID == AssignmentID);

            if (Assignment == null)
            {
                System.Diagnostics.Debug.WriteLine("No assignment with that ID found");
            }
         
            return Assignment;
        }

        /// <summary>
        /// Writes an assignment to the database
        /// </summary>
        /// <param name="a">Assignment</param>
        public void AddNewAssignment(Assignment a)
        {
            _db.Assignments.Add(a);
            _db.SaveChanges();
        }

        /// <summary>
        /// Save changes to database
        /// </summary>
        public void SaveToDatabase()
        {
            _db.SaveChanges();

        }

        /// <summary>
        /// Delete an assignment
        /// </summary>
        /// <param name="assignmentName">Assignment name</param>
        public void DeleteAssignmentWithName(string assignmentName)
        {

            var assignments = (from assignemt in _db.Assignments
                               where assignemt.Title == assignmentName
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

                    foreach (var submissionConnection in submissions)
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