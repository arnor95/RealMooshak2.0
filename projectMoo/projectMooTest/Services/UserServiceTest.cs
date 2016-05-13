using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using projectMoo.Services;
using projectMoo.BusinessLogicTest;
using projectMoo.Models.Entities;

namespace projectMooTest.Services
{
    [TestClass]
    public class UserServiceTest
    {

        private UserService _service;

        [TestInitialize]
        public void Initialize()
        {
            // Set up our mock database. In this case,
            // we only have to worry about one table
            // with 3 records:
            var mockDb = new MockDatabase();

            var u1 = new UserInfo()
            {
                ID = 1,
                Phone = "1111111",
                Name = "Gunnar",
                PicID = "profile.png",
                UserID = "5"
             };

            mockDb.UserInfoes.Add(u1);

            var u2 = new UserInfo()
            {
                ID = 2,
                Phone = "2222222",
                Name = "Hjalti",
                PicID = "profile.png",
                UserID = "6"
            };

            mockDb.UserInfoes.Add(u2);

            var u3 = new UserInfo()
            {
                ID = 3,
                Phone = "3333333",
                Name = "Ármann",
                PicID = "profile.png",
                UserID = "7"
            };

            mockDb.UserInfoes.Add(u3);

            var u4 = new UserInfo()
            {
                ID = 4,
                Phone = "4444444",
                Name = "Sverrir",
                PicID = "profile.png",
                UserID = "8"
            };

            mockDb.UserInfoes.Add(u4);

            var u5 = new UserInfo()
            {
                ID = 5,
                Phone = "5555555",
                Name = "Arnór",
                PicID = "profile.png",
                UserID = "9"
            };

            mockDb.UserInfoes.Add(u5);

            _service = new UserService(mockDb);

        }
        [TestMethod]
        public void GetUserInfoByID()
        {
            var userID = "9";

            var userInfo = _service.GetInfoForUser(userID);

            Assert.AreEqual("Arnór", userInfo.Name);
            Assert.AreNotEqual("1234567", userInfo.Phone);

        }

        [TestMethod]
        public void AddUserInfo()
        {
            var name = "Óli";
            var phone = "7777777";
            var userID = "7";

            _service.AddInfoForUser(name,phone,userID);

            var userInfo = _service.GetInfoForUser(userID);

            Assert.AreEqual(name, userInfo.Name);

        }
    }
}
