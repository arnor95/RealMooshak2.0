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
    }
}