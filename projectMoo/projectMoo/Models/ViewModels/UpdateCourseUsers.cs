using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class UpdateCourseUsers
    {
        [Display(Name = "User Email")]
        public string UserID { get; set; }
        public string Course { get; set; }
    }
}