using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class DeleteAssignment
    {
        [Required(ErrorMessage = "Please enter a name. If the name matches an assignment, the assignment will be deleted.")]
        [Display(Name = "Assignment name")]
        public string assignmentName { get; set; }
    }
}