using projectMoo.Models.Entities;
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
        public List<Submission> Submissions { get; set; }
    }
}