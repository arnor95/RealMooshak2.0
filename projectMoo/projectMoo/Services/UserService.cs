using projectMoo.Models;
using projectMoo.Models.Entities;
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

        public int getUserPhone(string userID)
        {
            var userInfo = (from user in _db.UserInfoes
                            where user.UserID == userID
                            select user).SingleOrDefault();

            return userInfo.Phone;
        }

        public string getUserPic(string userID)
        {
            var picID = (from info in _db.UserInfoes
                         where info.UserID == userID
                         select info).SingleOrDefault();

            return picID.PicID;
        }

        public void AddUserToGroup(string userId, string Group)
        {
            //Saves userId to a specific group

            UserGroup g = new UserGroup();
            g.GroupName = Group;
            g.UserID = userId;

            _db.UserGroups.Add(g);
            _db.SaveChanges();
        }
    }
}