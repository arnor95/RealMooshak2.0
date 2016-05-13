using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeDbSet;
using System.Data.Entity;
using projectMoo.Models;
using projectMoo.Models.Entities;
using projectMoo.Models.ViewModels;

namespace projectMoo.BusinessLogicTest
{
    /// <summary>
    /// This is an example of how we'd create a fake database by implementing the 
    /// same interface that the BookeStoreEntities class implements.
    /// </summary>
    public class MockDatabase : IAppDataContext
    {
        /// <summary>
        /// Sets up the fake database.
        /// </summary>
        public MockDatabase()
        {
            // We're setting our DbSets to be InMemoryDbSets rather than using SQL Server.
            Assignments            = new InMemoryDbSet<Assignment>();
            AssignmentMilestones   = new InMemoryDbSet<AssignmentMilestone>();
            Courses                = new InMemoryDbSet<Course>();
            UserCourses            = new InMemoryDbSet<UserCourse>();
            UserGroups             = new InMemoryDbSet<UserGroup>();
            Submissions            = new InMemoryDbSet<Submission>();
            UserInfoes             = new InMemoryDbSet<UserInfo>();
            MilestoneFinisheds     = new InMemoryDbSet<MilestoneFinished>();
        }

        public IDbSet<Assignment>              Assignments             { get; set; }
        public IDbSet<AssignmentMilestone>     AssignmentMilestones    { get; set; }
        public IDbSet<Course>                  Courses                 { get; set; }
        public IDbSet<UserCourse>              UserCourses             { get; set; }
        public IDbSet<UserInfo>                UserInfoes              { get; set; }
        public IDbSet<UserGroup>               UserGroups              { get; set; }
        public IDbSet<Submission>              Submissions             { get; set; }
        public IDbSet<MilestoneFinished>       MilestoneFinisheds      { get; set; }

        public int SaveChanges()
        {
            // Pretend that each entity gets a database id when we hit save.
            int changes = 0;
//          changes += DbSetHelper.IncrementPrimaryKey<Author>(x => x.AuthorId, this.Authors);
//          changes += DbSetHelper.IncrementPrimaryKey<Book>(x => x.BookId, this.Books);

            return changes;
        }

        public void Dispose()
        {
            // Do nothing!
        }
    }
}