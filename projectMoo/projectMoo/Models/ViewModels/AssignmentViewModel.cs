using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class AssignmentViewModel
    {
        public int AssignmentID { get; set; }
        public string Title { get; set; }
        public string CourseTitle { get; set; }

        [Display(Name = "Course")]
        public int CourseID { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        public List<AssignmentMilestoneViewModel> Milestones { get; set; }

        public AssignmentViewModel()
        {
            Milestones = new List<AssignmentMilestoneViewModel>();
            int maxMilestones = 10;

            for(int i = 0; i < maxMilestones ; i++)
            {
                var milestoneVM = new AssignmentMilestoneViewModel();
                Milestones.Add(milestoneVM);
            }
       
        }
    }
}