﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projectMoo.Models.Entities
{
    public class Assignment
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please select a course")]
        public int CourseID { get; set; }
        [Required(ErrorMessage = "Please enter a title for this assignment")]
        public string Title { get; set; }
        public decimal Grade { get; set; }
        [Required(ErrorMessage = "Please enter a descritption for this assignment")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please choose a due date for this assignment")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
    }
}