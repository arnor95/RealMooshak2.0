using Microsoft.AspNet.Identity;
using projectMoo.Models;
using projectMoo.Models.Entities;
using projectMoo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace projectMoo.Services
{
    public class MilestoneService
    {
        private IAppDataContext _db;
        private CourseService _courseService;
        private UserService _userService;

        public MilestoneService(IAppDataContext context)
        {
            _db = context ?? new ApplicationDbContext();
            _courseService = new CourseService(null);
            _userService = new UserService(null);
        }


        /// <summary>
        /// Gets all milestone for a specific assignment
        /// </summary>
        /// <param name="assignmentID">AssignmentID</param>
        /// <returns>List<AssignmentMilestoneViewModel></returns>
        public List<AssignmentMilestoneViewModel> GetMilestonesForAssignment(int assignmentID)
        {
            List<AssignmentMilestone> milestones = (from m in _db.AssignmentMilestones
                                                   where m.AssignmentID == assignmentID
                                                   select m).ToList();

            List<AssignmentMilestoneViewModel> returnMilestones = new List<AssignmentMilestoneViewModel>();
            string userID = HttpContext.Current.User.Identity.GetUserId();

            foreach (AssignmentMilestone m in milestones)
            {
                returnMilestones.Add(new AssignmentMilestoneViewModel
                {
                    MilestoneID = m.ID,
                    Title = m.Title,
                    Description = m.Description,
                    Status = _userService.HasFinishedMilestone(userID, m.ID),
                    Grade = m.Grade,
                    Percentage = m.Percentage
                });
            }

            return returnMilestones;
        }


        /// <summary>
        /// Get a milestone by its ID
        /// </summary>
        /// <param name="ID">MilestoneID</param>
        /// <returns>AssignmentMilestoneViewModel</returns>
        public AssignmentMilestoneViewModel GetMilestoneByID(int ID)
        {
            AssignmentMilestone milestone = (from m in _db.AssignmentMilestones
                                             where m.ID == ID
                                             select m).SingleOrDefault();

            AssignmentMilestoneViewModel returnModel = new AssignmentMilestoneViewModel();
            returnModel.Title = milestone.Title;
            returnModel.Percentage = milestone.Percentage;
            returnModel.Grade = milestone.Grade;
            returnModel.Description = milestone.Description;
            returnModel.MilestoneID = milestone.ID;

            return returnModel;
        }


        /// <summary>
        /// Writes a list of milestones to the database and assigns it to an assignment
        /// </summary>
        /// <param name="assignment">AssignmentID</param>
        /// <param name="milestonesVM">List of milestones</param>
        public void AddMilestonesForAssignment(int assignment, List<AssignmentMilestoneViewModel> milestonesVM)
        {
            foreach (AssignmentMilestoneViewModel milestoneVM in milestonesVM)
            {
                if(milestoneVM.Title != "" && milestoneVM.Percentage != 0 && milestoneVM.Description != "" && !(milestoneVM.Input == "") && !(milestoneVM.Output == ""))
                {
                    AssignmentMilestone milestone = new AssignmentMilestone();
                    milestone.Description = milestoneVM.Description;
                    milestone.Title = milestoneVM.Title;
                    milestone.Grade = 0;
                    milestone.Percentage = milestoneVM.Percentage;
                    milestone.AssignmentID = assignment;

                    _db.AssignmentMilestones.Add(milestone);
                    _db.SaveChanges();

                    int milestoneID = milestone.ID;
                    System.Diagnostics.Debug.WriteLine(milestoneID);

                    string newFolderPath = System.Web.HttpContext.Current.Server.MapPath("~/Code/Teacher/" + milestoneID + "/");
                    Directory.CreateDirectory(newFolderPath);

                    string input = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Code/Teacher/" + milestoneID + "/"), "input.txt");
                    string output = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Code/Teacher/" + milestoneID + "/"), "output.txt");
                    string codePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Code/Teacher/" + milestoneID + "/"), "code.cpp");

                    if (!Directory.Exists(input))
                    {
                        Directory.CreateDirectory("C:\\Test");
                    }
                    using (StreamWriter writer = new StreamWriter(input, true, Encoding.Default))
                    {    
                        writer.WriteLine(milestoneVM.Input);
                    }
                    using (StreamWriter writer = new StreamWriter(output, true, Encoding.Default))
                    {
                        writer.WriteLine(milestoneVM.Output);
                    }
                }
            }
        }


        /// <summary>
        /// Saves the changes to the database
        /// </summary>
        public void SaveToDatabase()
        {
            _db.SaveChanges();
        }
    }
}