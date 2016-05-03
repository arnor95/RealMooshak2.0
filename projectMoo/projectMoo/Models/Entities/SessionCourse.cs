using projectMoo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Models.Entities
{
    public class SessionCourse
    {
        private CourseViewModel _activeCourse { get; set; }


        private static SessionCourse theInstance = null;

        public static SessionCourse Instance
        {
            get
            {
                if (theInstance == null)
                {
                    theInstance = new SessionCourse();

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