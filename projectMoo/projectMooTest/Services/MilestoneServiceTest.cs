using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using projectMoo.BusinessLogicTest;
using projectMoo.Models.Entities;
using projectMoo.Models.ViewModels;
using System.Collections.Generic;

namespace projectMooTest.Services
{
    [TestClass]
    public class MilestoneServiceTest
    {
        private projectMoo.Services.MilestoneService _service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDatabase();

            var milestone1 = new AssignmentMilestone()
            {
                ID = 1,
                AssignmentID = 10,
                Description = "Write a program which return the sum of two numbers",
                Grade = 0,
                Percentage = 15,
                Title = "Milestone 1"
            };
            mockDb.AssignmentMilestones.Add(milestone1);

            var milestone2 = new AssignmentMilestone()
            {
                ID = 2,
                AssignmentID = 10,
                Description = "Write a blackjack",
                Grade = 0,
                Percentage = 35,
                Title = "Milestone 2"
            };
            mockDb.AssignmentMilestones.Add(milestone2);

            var milestone3 = new AssignmentMilestone()
            {
                ID = 3,
                AssignmentID = 10,
                Description = "Write a bank account",
                Grade = 0,
                Percentage = 50,
                Title = "Milestone 3"
            };
            mockDb.AssignmentMilestones.Add(milestone3);

            _service = new projectMoo.Services.MilestoneService(mockDb);
        }

        [TestMethod]
        public void GetMilestoneByID()
        {
            // Arrange: 
            const int ID = 1;

            // Act:
            AssignmentMilestoneViewModel milestone = _service.GetMilestoneByID(ID);

            // Assert:
            Assert.AreEqual("Milestone 1", milestone.Title);
            Assert.AreEqual("Write a program which return the sum of two numbers", milestone.Description);
            Assert.AreEqual(15, milestone.Percentage);
            Assert.AreEqual(0, milestone.Grade);
        }
    }
}
