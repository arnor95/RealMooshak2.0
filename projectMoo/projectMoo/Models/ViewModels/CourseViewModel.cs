using projectMoo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class CourseViewModel
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public List<Assignment> Assignments { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}