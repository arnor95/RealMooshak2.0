using projectMoo.Models;
using projectMoo.Models.Entities;
using projectMoo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Services
{
    public class MilestoneService
    {
        private ApplicationDbContext _db;
        private CoursesService _courseService;

        public MilestoneService()
        {
            _db = new ApplicationDbContext();
            _courseService = new CoursesService();
        }

        public List<AssignmentMilestoneViewModel> getMilestonesForAssignment(int assignmentID)
        {
            List<AssignmentMilestone> milestones = (from m in _db.AssignmentMilestones
                                                   where m.AssignmentID == assignmentID
                                                   select m).ToList();

            List<AssignmentMilestoneViewModel> returnMilestones = new List<AssignmentMilestoneViewModel>();

            foreach (AssignmentMilestone m in milestones)
            {
                returnMilestones.Add(new AssignmentMilestoneViewModel
                {
                    MilestoneID = m.ID,
                    Title = m.Title,
                    Description = m.Description,
                    Grade = m.Grade,
                    Percentage = m.Percentage
                });
            }

            return returnMilestones;
        }

        public AssignmentMilestoneViewModel getMilestoneByID(int ID)
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
    }
}