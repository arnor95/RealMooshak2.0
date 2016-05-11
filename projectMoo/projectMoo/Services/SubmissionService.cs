using Microsoft.AspNet.Identity;
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

        public List<Submission> getAllSubmissionsByMilestoneIDForUser(int ID)
        {
            string userID = HttpContext.Current.User.Identity.GetUserId();

            List<Submission> submissions = (from s in _db.Submissions
                                            where s.MilestoneID == ID && s.UserID == userID
                                            select s).ToList();

            return submissions;
        }

        public List<Submission> getAllSubmissionsByMilestoneID(int ID)
        {
            List<Submission> submissions = (from s in _db.Submissions
                                            where s.MilestoneID == ID
                                            select s).ToList();

            return submissions;
        }

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

                            lines.Add(myStreamReader.ReadLine());

                            if(outputs[i] != lines[i])
                            {
                                returnModel.Status = false;
                            }

                            returnModel.Input.Add(inputs[i]);
                            returnModel.Output.Add(lines[i]);
                            returnModel.ExpectedOutput.Add(outputs[i]);
                        }


                    }


                }
                //ViewBag.Success = lines;

                // TODO: We might want to clean up after the process, there
                // may be files we should delete etc.
                
            }

            return returnModel;
        }
    }
}