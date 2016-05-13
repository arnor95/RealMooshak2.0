using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using projectMoo.Models.Entities;

namespace projectMoo.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public interface IAppDataContext
    {
        IDbSet<Assignment>              Assignments             { get; set; }
        IDbSet<AssignmentMilestone>     AssignmentMilestones    { get; set; }
        IDbSet<Course>                  Courses                 { get; set; }
        IDbSet<UserCourse>              UserCourses             { get; set; }
        IDbSet<UserInfo>                UserInfoes              { get; set; }
        IDbSet<UserGroup>               UserGroups              { get; set; }
        IDbSet<Submission>              Submissions             { get; set; }
        IDbSet<MilestoneFinished>       MilestoneFinisheds      { get; set; }

        int SaveChanges();
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IAppDataContext
    {
        public IDbSet<Assignment>            Assignments             { get; set; }
        public IDbSet<AssignmentMilestone>   AssignmentMilestones    { get; set; }
        public IDbSet<Course>                Courses                 { get; set; }
        public IDbSet<UserCourse>            UserCourses             { get; set; }
        public IDbSet<UserInfo>              UserInfoes              { get; set; }
        public IDbSet<UserGroup>             UserGroups              { get; set; }
        public IDbSet<Submission>            Submissions             { get; set; }
        public IDbSet<MilestoneFinished>     MilestoneFinisheds      { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}