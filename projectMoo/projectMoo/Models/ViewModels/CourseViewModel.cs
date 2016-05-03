using projectMoo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class CourseViewModel
    {
        public string Title { get; set; }
        public List<Assignment> Assignments { get; set; }
        public string Description { get; set; }
    }
}