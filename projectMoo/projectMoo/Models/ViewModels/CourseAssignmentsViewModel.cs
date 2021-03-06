﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class CourseAssignmentsViewModel
    {
        public string Name { get; set; }
        public int Active { get; set; }
        public List<CourseViewModel> Courses { get; set; }
        public CourseViewModel ActiveCourse { get; set; }
        public CourseViewModel Course { get; set; }
        public List<AssignmentViewModel> Assignments { get; set; }
    }
}