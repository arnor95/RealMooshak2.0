using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class SubmissionsForTeacherViewModel
    {
        public string UserName { get; set; }
        public bool Status { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
    }
}