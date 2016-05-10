using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class DeleteCourseViewModel
    {
        [Required(ErrorMessage = "Please enter a name. If the name matches a course, the course will be deleted.")]
        public string courseName { get; set; }
    }
}