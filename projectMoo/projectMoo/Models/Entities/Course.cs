using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projectMoo.Models.Entities
{
    public class Course
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please enter a title for this course")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter a description for this course")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string Group { get; set; }


        // TODO: Begin and end date
    }
}