using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class DeleteUserViewModel
    {
        [Display(Name = "User email")]
        public string userEmail { get; set; }

    }
}