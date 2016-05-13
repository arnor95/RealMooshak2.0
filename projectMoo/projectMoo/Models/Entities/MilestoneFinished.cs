using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.Entities
{
    public class MilestoneFinished
    {
        public int ID { get; set; }
        public int MilestoneID { get; set; }
        public string UserID { get; set; }
    }
}