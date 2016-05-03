using projectMoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Services
{
    public class UserService
    {
        private ApplicationDbContext _db;

        public UserService()
        {
            _db = new ApplicationDbContext();
        }

        public string getUserName(string userID)
        {
            var userInfo = (from user in _db.UserInfoes
                            where user.UserID == userID
                            select user).SingleOrDefault();

            return userInfo.Name;
        }
    }
}