using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using projectMoo.BusinessLogicTest;
using projectMoo.Models.Entities;
using projectMoo.Services;
using projectMoo.Models.ViewModels;
using System.Collections.Generic;

namespace projectMooTest.Services
{
    [TestClass]
    public class CourseServiceTest
    {
        private CoursesService _service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDatabase();

            var course1 = new Course()
            {
                ID = 5,
                Description = "Brutal course",
                BeginDate = new DateTime().Date,
                EndDate = new DateTime().Date,
                Group = "1. árs nemar",
                Title = "Gagnaskipan"
            };
            mockDb.Courses.Add(course1);

            var course2 = new Course()
            {
                ID = 4,
                Description = "Thats one tough course",
                BeginDate = new DateTime().Date,
                EndDate = new DateTime().Date,
                Group = "1. árs nemar",
                Title = "Vefforritun"
            };
            mockDb.Courses.Add(course2);

            var course3 = new Course()
            {
                ID = 3,
                Description = "Mental course",
                BeginDate = new DateTime().Date,
                EndDate = new DateTime().Date,
                Group = "1. árs nemar",
                Title = "Forritun"
            };
            mockDb.Courses.Add(course3);

            _service = new CoursesService(mockDb);
        }

        [TestMethod]
        public void GetCourseByID()
        {
            // Arrange: 
            const int ID = 5;

            // Act:
            CourseViewModel course = _service.GetCourseByID(ID);

            // Assert:
            Assert.AreEqual("Gagnaskipan", course.Title);
            Assert.AreEqual("Brutal course", course.Description);
        }

        [TestMethod]
        public void GetAllCourses()
        {
            // Act:
            List<Course> courses = _service.GetAllCourses();

            // Assert:
            Assert.AreEqual(3, courses.Count);
            Assert.AreEqual(5, courses[0].ID);
            Assert.AreEqual("Vefforritun", courses[1].Title);
            Assert.AreEqual("Mental course", courses[2].Description);
        }
    }
}
