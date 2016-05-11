using projectMoo.Models;
using projectMoo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace projectMoo.Services
{
    public class UserService
    {
        private ApplicationDbContext _db;
        
        public UserService()
        {
            _db = new ApplicationDbContext();
        }

        public UserInfo getInfoForUser(string userID)
        {
            UserInfo info = (from user in _db.UserInfoes
                             where user.UserID == userID
                             select user).SingleOrDefault();

            if (info == null)
            {
                return new UserInfo();
            }

            return info;

        }

        public string getUserName(string userID)
        {
            var userInfo = (from user in _db.UserInfoes
                            where user.UserID == userID
                            select user).SingleOrDefault();

            return userInfo.Name;
        }

        public void DeleteConnectionsToUser(string userID)
        {
            var userGroups = _db.UserGroups.Where(a => a.UserID == userID).ToList();
            foreach (var connection in userGroups)
            {
                _db.UserGroups.Remove(connection);

            }

            var userCourses = _db.UserCourses.Where(a => a.UserID == userID).ToList();
            foreach (var connection in userCourses)
            {
                _db.UserCourses.Remove(connection);

            }

            var userInfo = _db.UserInfoes.Where(a => a.UserID == userID).ToList();
            foreach (var connection in userInfo)
            {
                _db.UserInfoes.Remove(connection);

            }

            var userSubmissions = _db.Submissions.Where(a => a.UserID == userID).ToList();
            foreach (var connection in userSubmissions)
            {
                _db.Submissions.Remove(connection);

            }

            _db.SaveChanges();
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
        }

        public void AddInfoForUser(string name, int phone, string userID)
        {
            UserInfo info = new UserInfo();
            info.Phone = phone;
            info.Name = name;
            info.PicID = "profile.png";
            info.UserID = userID;
            _db.UserInfoes.Add(info);

        }

        public void SaveToDatabase()
        {
            _db.SaveChanges();

        }

        public async void DeleteUserByEmail(string Email, UserManager<ApplicationUser> manager)
        {
           
            var user = await manager.FindByEmailAsync(Email);
            var rolesForUser = await manager.GetRolesAsync(user.Id);

            if (rolesForUser.Count() > 0)
            {
                foreach (var item in rolesForUser.ToList())
                {
                    // item should be the name of the role
                    var result = await manager.RemoveFromRoleAsync(user.Id, item);
                }
            }

            await manager.DeleteAsync(user);

        }
    }
}