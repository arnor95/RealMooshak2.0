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
        private ApplicationDbContext _db;
        private CoursesService _courseService;
        private MilestoneService _milestoneService;

        public AssignmentsService()
        {
            _db = new ApplicationDbContext();
            _courseService = new CoursesService();
            _milestoneService = new MilestoneService();
        }

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

                courses.Add(userCourse);
            }

            foreach (Course item in courses)
            {
                System.Diagnostics.Debug.WriteLine(item.Title);
                returnList.AddRange(GetAssignmentsInCourse(item.ID));
            }

            return returnList;
        }

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
            
            foreach (var assign in Assignments)
            {
                System.Diagnostics.Debug.WriteLine(assign.CourseID);

                listAssignments.Add(new AssignmentViewModel
                {
                    Title = assign.Title,
                    CourseTitle = _courseService.getCourseByID(assign.CourseID).Title.ToString(),
                    Description = assign.Description,
                    Milestones = _milestoneService.getMilestonesForAssignment(assign.ID)
                });
            }

            return listAssignments;
        }

        public AssignmentViewModel GetAssignmentByID(int AssignmentID)
        {
            var Assignment = _db.Assignments.SingleOrDefault(x => x.ID == AssignmentID);

            if (Assignment == null)
            {
                System.Diagnostics.Debug.WriteLine("No assignment with that ID found");
            }
            /*
            var milestones = _db.AssignmentMilestones
                .Where(x => x.AssignmentID == AssignmentID)
                .Select(x => new AssignmentMilestoneViewModel
                {
                    Title = x.Title,
                    Description = x.Description,
                    Grade = x.Grade,
                    Percentage = x.Percentage
                })
                .ToList();
            */
            var viewModel = new AssignmentViewModel
            {
                Title = Assignment.Title,
                //Milestones = milestones
            };
            
            return viewModel;
        }
    }
}