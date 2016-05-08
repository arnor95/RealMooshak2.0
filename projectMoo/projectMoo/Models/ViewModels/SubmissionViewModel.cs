using projectMoo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class SubmissionViewModel
    {
        public AssignmentMilestoneViewModel Milestone { get; set; }
        public int Percentage { get; set; }
        public bool Status { get; set; }
        public string Output { get; set; }
    }
}