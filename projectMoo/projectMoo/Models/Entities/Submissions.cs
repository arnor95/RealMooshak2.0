﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.Entities
{
    public class Submissions
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int MilestoneID { get; set; }
        public bool State { get; set; }
        public string FileID { get; set; }
    }
}