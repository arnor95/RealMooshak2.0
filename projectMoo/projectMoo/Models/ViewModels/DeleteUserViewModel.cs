using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class DeleteUserViewModel
    {
        [Display(Name = "User Email")]
        public string userEmail { get; set; }

    }
}