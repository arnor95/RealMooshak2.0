﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.ViewModels
{
    public class UserRole
    {
        public string Username { get; set; }
        public string Roles { get; set; }
        public string UserId { get; set; }
        public bool Selected { get; set; }
    }
}