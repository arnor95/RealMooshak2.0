using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class NewUserViewModel
    {
        [Required(ErrorMessage = "Please add a valid RU email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Give this user a default password.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please add a role for this user Admin/Teacher/Student.")]
        public string Role { get; set; }

        public string Group { get; set; }

        [Required(ErrorMessage = "Please add a name for this user.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please add a phone number for this user.")]
        public string Phone { get; set; }


    }
}