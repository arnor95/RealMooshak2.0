using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeDbSet;
using System.Data.Entity;
using projectMoo.Models;
using projectMoo.Models.Entities;

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
            this.Assignments            = new InMemoryDbSet<Assignment>();
            this.AssignmentMilestones   = new InMemoryDbSet<AssignmentMilestone>();
            this.Courses                = new InMemoryDbSet<Course>();
            this.UserCourses            = new InMemoryDbSet<UserCourse>();
            this.UserGroups             = new InMemoryDbSet<UserGroup>();
            this.Submissions            = new InMemoryDbSet<Submission>();
        }

        IDbSet<Assignment>              Assignments             { get; set; }
        IDbSet<AssignmentMilestone>     AssignmentMilestones    { get; set; }
        IDbSet<Course>                  Courses                 { get; set; }
        IDbSet<UserCourse>              UserCourses             { get; set; }
        IDbSet<UserInfo>                UserInfoes              { get; set; }
        IDbSet<UserGroup>               UserGroups              { get; set; }
        IDbSet<Submission>              Submissions             { get; set; }

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