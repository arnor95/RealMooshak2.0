using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.Entities
{
    public class AssignmentMilestone
    {
        public int ID { get; set; }
        public int AssignmentID { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public decimal Grade { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Determines how much this milestone weights in the assignment.
        /// Example: if this milestone is 15% of the grade of the assignment
        /// then this property contains the value 15.
        /// </summary>
        public int Percentage { get; set; }
    }
}  