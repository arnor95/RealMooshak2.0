using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using projectMoo.Services;
using projectMoo.BusinessLogicTest;
using projectMoo.Models.Entities;
using System.Collections.Generic;
using projectMoo.Models.ViewModels;

namespace projectMooTest.Services
{
    [TestClass]
    public class CourseService
    {

        private AssignmentsService _service;

        [TestInitialize]
        public void Initialize()
        {
            // Set up our mock database. In this case,
            // we only have to worry about one table
            // with 3 records:
            var mockDb = new MockDatabase();

            var a1 = new Assignment()
            {
                ID = 10,
				Description = "Really hard task.",
				CourseID = 5,
                Title = "Gagnaskipan week 3",
                DueDate = new DateTime().Date,
                Grade = 10

            };

            mockDb.Assignments.Add(a1);

            var a2 = new Assignment()
            {
                ID = 11,
                Description = "Really hard task too.",
                CourseID = 5,
                Title = "Gagnaskipan week 4",
                DueDate = new DateTime().Date,
                Grade = 10

            };

            mockDb.Assignments.Add(a2);

            var a3 = new Assignment()
            {
                ID = 12,
                Description = "Really hard task also.",
                CourseID = 5,
                Title = "Gagnaskipan week 5",
                DueDate = new DateTime().Date,
                Grade = 10


            };

            mockDb.Assignments.Add(a3);

            var a4 = new Assignment()
            {
                ID = 13,
                Description = "Really hard task also too.",
                CourseID = 4,
                Title = "Vefforritun week 1",
                DueDate = new DateTime().Date,
                Grade = 10


            };

            mockDb.Assignments.Add(a4);

            _service = new AssignmentsService(mockDb);

        }

        [TestMethod]
        public void GetAssignmentById()
        {

            // Arrange:
            const int aID = 10;

            // Act:
            var assignment = _service.GetAssignmentByID(aID);

            // Assert:
            Assert.AreEqual("Gagnaskipan week 3", assignment.Title);
            Assert.AreEqual("Really hard task.", assignment.Description);
        }

        [TestMethod]
        public void GetAssignmentsInCourse()
        {

            // Arrange:
            const int cID = 4;

            // Act:
            List<AssignmentViewModel> assignments = _service.GetAssignmentsInCourse(cID);

            // Assert:
            Assert.AreEqual(1, assignments.Count);

        }
        [TestMethod]
        public void AddAssignment()
        {

           var newAssignment = new Assignment()
            {
                ID = 14,
                Description = "new Really hard task.",
                CourseID = 4,
                Title = "Vefforritun week 2",
                DueDate = new DateTime().Date,
                Grade = 10


            };

            _service.AddNewAssignment(newAssignment);

            Assert.AreEqual(5, _service.GetAllAssignments().Count);
       
        }

        [TestMethod]
        public void GetAllAssignments()
        {
            var assignmentCount = 4;

            var assignments = _service.GetAllAssignments();

            Assert.AreEqual(assignmentCount, assignments.Count);
        }
    }
}
