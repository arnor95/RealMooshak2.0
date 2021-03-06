﻿using Microsoft.AspNet.Identity;
using projectMoo.Models;
using projectMoo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using projectMoo.Models.ViewModels;
using System.Diagnostics;
using System.IO;

namespace projectMoo.Services
{
    public class SubmissionService
    {
        private IAppDataContext _db;
        private UserService _userService;

        public SubmissionService(IAppDataContext context)
        {
            _db = context ?? new ApplicationDbContext();
            _userService = new UserService(null);
        }


        /// <summary>
        /// Write a submission to the database
        /// </summary>
        /// <param name="data">Submission</param>
        public void SubmitSubmission(Submission data)
        {
            _db.Submissions.Add(data);
            _db.SaveChanges();
        }


        /// <summary>
        /// Get all submission for a milestone based on the userID
        /// </summary>
        /// <param name="ID">UserID</param>
        /// <returns>List<Submission></returns>
        public List<Submission> GetAllSubmissionsByMilestoneIDForUser(int ID)
        {
            string userID = HttpContext.Current.User.Identity.GetUserId();

            List<Submission> submissions = (from s in _db.Submissions
                                            where s.MilestoneID == ID && s.UserID == userID
                                            orderby s.State descending
                                            select s).ToList();

            return submissions;
        }


        /// <summary>
        /// Get all submissions for a milestone
        /// </summary>
        /// <param name="ID">MilestoneID</param>
        /// <returns>List<Submission></returns>
        public List<Submission> GetAllSubmissionsByMilestoneID(int ID)
        {
            List<Submission> submissions = (from s in _db.Submissions
                                            where s.MilestoneID == ID
                                            orderby s.State descending
                                            select s).ToList();

            return submissions;
        }


        /// <summary>
        /// Compile a code that the user submits
        /// </summary>
        /// <param name="workingFolder">Working Directory</param>
        /// <param name="fileName">Filename</param>
        /// <param name="milestoneID">MilestoneID</param>
        /// <returns>ResultViewModel</returns>
        public ResultViewModel CompileCode(string workingFolder, string fileName, int milestoneID)
        {
            ResultViewModel returnModel = new ResultViewModel
            {
                Status = true,
                Input = new List<string>(),
                Output = new List<string>(),
                ExpectedOutput = new List<string>()
            };

            #region SETUP
            string cppFileName = fileName + ".cpp";

            // Set up our working folder, and the file names/paths.
            // In this example, this is all hardcoded, but in a
            // real life scenario, there should probably be individual
            // folders for each user/assignment/milestone.
            workingFolder = workingFolder + "\\";

            var exeFilePath = workingFolder + fileName + ".exe";

            var inputFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Code/Teacher/" + milestoneID + "/input.txt");
            var outputFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Code/Teacher/" + milestoneID + "/output.txt");
            
            // In this case, we use the C++ compiler (cl.exe) which ships
            // with Visual Studio. It is located in this folder:
            var compilerFolder = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\VC\\bin\\";
            // There is a bit more to executing the compiler than
            // just calling cl.exe. In order for it to be able to know
            // where to find #include-d files (such as <iostream>),
            // we need to add certain folders to the PATH.
            // There is a .bat file which does that, and it is
            // located in the same folder as cl.exe, so we need to execute
            // that .bat file first.

            // Using this approach means that:
            // * the computer running our web application must have
            //   Visual Studio installed. This is an assumption we can
            //   make in this project.
            // * Hardcoding the path to the compiler is not an optimal
            //   solution. A better approach is to store the path in
            //   web.config, and access that value using ConfigurationManager.AppSettings.

            #endregion

            #region COMPILE
            // Execute the compiler:
            Process compiler = new Process();
            compiler.StartInfo.FileName = "cmd.exe";
            compiler.StartInfo.WorkingDirectory = workingFolder;
            compiler.StartInfo.RedirectStandardInput = true;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.UseShellExecute = false;

            compiler.Start();
            compiler.StandardInput.WriteLine("\"" + compilerFolder + "vcvars32.bat" + "\"");
            compiler.StandardInput.WriteLine("cl.exe /nologo /EHsc " + cppFileName);
            compiler.StandardInput.WriteLine("exit");
            string output = compiler.StandardOutput.ReadToEnd();
            compiler.WaitForExit();
            compiler.Close();
            #endregion

            Stopwatch sw = Stopwatch.StartNew();

            // Check if the compile succeeded, and if it did,
            // we try to execute the code:
            if (System.IO.File.Exists(exeFilePath))
            {
                var logInput = System.IO.File.ReadAllLines(inputFile);
                var inputs = new List<string>(logInput);
                var logOutput = System.IO.File.ReadAllLines(outputFile);
                var outputs = new List<string>(logOutput);

                var processInfoExe = new ProcessStartInfo(exeFilePath, "");

                processInfoExe.UseShellExecute = false;
                processInfoExe.RedirectStandardOutput = true;
                processInfoExe.RedirectStandardInput = true;
                processInfoExe.RedirectStandardError = true;
                processInfoExe.CreateNoWindow = true;
                var lines = new List<string>();
                
                for (int i = 0; i < inputs.Count; i++)
                {
                    using (var processExe = new Process())
                    {
                        processExe.StartInfo = processInfoExe;
                        processExe.Start();
                        StreamWriter myStreamWriter = processExe.StandardInput;
                        StreamReader myStreamReader = processExe.StandardOutput;
                        
                        myStreamWriter.WriteLine(inputs[i]);

                        // We then read the output of the program:
                        while (!processExe.StandardOutput.EndOfStream)
                        {
                            if (sw.Elapsed.Seconds > 30)
                            {
                                sw.Stop();
                                processExe.Close();
                                returnModel.Input.Add("Compiler timed out");
                                returnModel.ExpectedOutput.Add("Compiler timed out");
                                returnModel.Output.Add("Compiler timed out");
                                returnModel.Status = false;
                                return (returnModel);
                            }

                            lines.Add(myStreamReader.ReadLine());

                            if(outputs[i] != lines[i])
                            {
                                returnModel.Status = false;
                            }

                            returnModel.Input.Add(inputs[i]);
                            returnModel.Output.Add(lines[i]);
                            returnModel.ExpectedOutput.Add(outputs[i]);
                        }
                        processExe.Close();
                        processExe.Kill();
                    }
                } 
            }
            
            return returnModel;
        }


        /// <summary>
        /// Returns all submissions for a teacher based for a specific milestone
        /// </summary>
        /// <param name="milestoneID">MilestoneID</param>
        /// <returns>List<SubmissionsForTeacherViewModel></returns>
        public List<SubmissionsForTeacherViewModel> GetSubmissionsForTeacherByMilestoneID(int milestoneID)
        {
            List<Submission> submissions = (from s in _db.Submissions
                                            where s.MilestoneID == milestoneID
                                            orderby s.State descending
                                            select s).ToList();

            List<SubmissionsForTeacherViewModel> teacherSubmissions = new List<SubmissionsForTeacherViewModel>();

            foreach (var s in submissions)
            {
                teacherSubmissions.Add(new SubmissionsForTeacherViewModel
                {
                    FileName = s.FileID,
                    Status = s.State,
                    UserName = _userService.GetUserName(s.UserID),
                    Date = s.Date
                });
            }

            return teacherSubmissions;
        }
    }
}