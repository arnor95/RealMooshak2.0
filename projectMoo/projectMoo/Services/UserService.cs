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
        private IAppDataContext _db;
        
        public UserService(IAppDataContext context)
        {
            _db = context ?? new ApplicationDbContext();
        }


        /// <summary>
        /// Returns info about a specific user
        /// </summary>
        /// <param name="userID">UserID</param>
        /// <returns>UserInfo</returns>
        public UserInfo GetInfoForUser(string userID)
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


        /// <summary>
        /// Returns the name of a specific user
        /// </summary>
        /// <param name="userID">UserID</param>
        /// <returns>Users full name</returns>
        public string GetUserName(string userID)
        {
            UserInfo userInfo = (from user in _db.UserInfoes
                            where user.UserID == userID
                            select user).SingleOrDefault();

            return userInfo.Name;
        }


        /// <summary>
        /// Deletes all connection the user has in the database
        /// </summary>
        /// <param name="userID">UserID</param>
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


        /// <summary>
        /// Returns the phone number for a specific user
        /// </summary>
        /// <param name="userID">UserID</param>
        /// <returns>Users phone number</returns>
        public string GetUserPhone(string userID)
        {
            UserInfo userInfo = (from user in _db.UserInfoes
                            where user.UserID == userID
                            select user).SingleOrDefault();

            return userInfo.Phone;
        }


        /// <summary>
        /// Returns a path to the users profile picture
        /// </summary>
        /// <param name="userID">UserID</param>
        /// <returns>Path to users profile picture</returns>
        public string GetUserPic(string userID)
        {
            UserInfo picID = (from info in _db.UserInfoes
                         where info.UserID == userID
                         select info).SingleOrDefault();

            return picID.PicID;
        }


        /// <summary>
        /// Add a specific user to a specific group
        /// </summary>
        /// <param name="userId">UserID</param>
        /// <param name="Group">Group name</param>
        public void AddUserToGroup(string userId, string Group)
        {
            //Saves userId to a specific group

            UserGroup g = new UserGroup();
            g.GroupName = Group;
            g.UserID = userId;

            _db.UserGroups.Add(g);
        }


        /// <summary>
        /// Writes info for a specific user to the database
        /// </summary>
        /// <param name="name">Users full name</param>
        /// <param name="phone">Users phone number</param>
        /// <param name="userID">UserID</param>
        public void AddInfoForUser(string name, string phone, string userID)
        {
            UserInfo info = new UserInfo();
            info.Phone = phone;
            info.Name = name;
            info.PicID = "profile.png";
            info.UserID = userID;
            _db.UserInfoes.Add(info);

        }


        /// <summary>
        /// Save changes to the database
        /// </summary>
        public void SaveToDatabase()
        {
            _db.SaveChanges();

        }


        /// <summary>
        /// Deletes a user from the database based on his email
        /// </summary>
        /// <param name="Email">Email</param>
        /// <param name="manager">UserManager</param>
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

        public bool HasFinishedMilestone(string userID, int milestoneID)
        {
            var milestone = (from b in _db.MilestoneFinisheds
                             where b.UserID == userID && b.MilestoneID == milestoneID
                             select b).SingleOrDefault();

            if (milestone != null)
                return true;

            return false;
        }
    }
}