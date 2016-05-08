using projectMoo.Models;
using projectMoo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projectMoo.Services
{
    public class SubmissionService
    {
        private ApplicationDbContext _db;

        public SubmissionService()
        {
            _db = new ApplicationDbContext();
        }

        public void SubmitSubmission(Submission data)
        {
            _db.Submissions.Add(data);
            _db.SaveChanges();
        }

        public List<Submission> getAllSubmissionsByMilestoneID(int ID)
        {
            List<Submission> submissions = (from s in _db.Submissions
                                            where s.MilestoneID == ID
                                            select s).ToList();

            return submissions;
        }
    }
}