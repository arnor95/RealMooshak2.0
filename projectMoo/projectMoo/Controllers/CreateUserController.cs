using Microsoft.AspNet.Identity;
using projectMoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace projectMoo.Controllers
{
    public class CreateUserController : Controller
    {
        // GET: CreateUser
        public ActionResult Index()
        {
            /*
            var user = new ApplicationUser { UserName = "hjalti15@ru.is", Email = "hjalti15@ru.is" };
            Microsoft.AspNet.Identity.UserManage;
           */
            return View();
        }
    }
}