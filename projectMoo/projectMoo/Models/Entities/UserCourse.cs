using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.Entities
{
    public class UserCourse
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int CourseID { get; set; }
    }
}