using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class ResultViewModel
    {
        public bool Status { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string ExpectedOutput { get; set; }
    }
}