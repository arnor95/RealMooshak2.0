using projectMoo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class AddCourseViewModel
    {
        public Course course { get; set; }
        public List<UserRole> Teachers { get; set; }
        public List<UserRole> Students { get; set; }


    }
}