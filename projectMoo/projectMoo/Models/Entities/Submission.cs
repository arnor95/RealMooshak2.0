using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.Entities
{
    public class Submission
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int MilestoneID { get; set; }
        public bool State { get; set; }
        public string FileID { get; set; }
    }
}