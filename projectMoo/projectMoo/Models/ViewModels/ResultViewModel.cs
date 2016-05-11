using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class ResultViewModel
    {
        public bool Status { get; set; }
        public List<string> Input { get; set; }
        public List<string> Output { get; set; }
        public List<string> ExpectedOutput { get; set; }
    }
}