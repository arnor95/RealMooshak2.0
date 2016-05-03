using projectMoo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.Entities
{
    public class ActiveCourse
    {
        private CourseViewModel _activeCourse { get; set; }
 

        private static ActiveCourse theInstance = null;

        public static ActiveCourse Instance
        {
            get
            {
                if (theInstance == null)
                {
                    theInstance = new ActiveCourse();

                }
                return theInstance;
            }
        }

        public void SetActiveCourse(CourseViewModel c)
        {
            _activeCourse = c;
        }

        public CourseViewModel GetActiveCourse()
        {
            return _activeCourse;
        }
    }
}