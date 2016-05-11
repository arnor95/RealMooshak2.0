using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class AssignmentMilestoneViewModel
    {
        public Guid UniqueId { get; set; }
        public int MilestoneID { get; set; }
        public string Title { get; set; }
        public decimal Grade { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Determines how much this milestone weights in the assignment.
        /// Example: if this milestone is 15% of the grade of the assignment
        /// then this property contains the value 15.
        /// </summary>
        public int Percentage { get; set; }
        public List<string> Output { get; set; }
        public List<string> Input { get; set; }

        public AssignmentMilestoneViewModel()
        {
            UniqueId = Guid.NewGuid();
            Input = new List<string>();
            Input.Add("");
            Output = new List<string>();
            Output.Add("");
        }
    }
}