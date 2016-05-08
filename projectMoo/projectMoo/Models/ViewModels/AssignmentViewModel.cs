using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class AssignmentViewModel
    {
        public string Title { get; set; }
        public string CourseTitle { get; set; }
        public int CourseID { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
        public List<AssignmentMilestoneViewModel> Milestones { get; set; }
    }
}