using projectMoo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projectMoo.Handlers
{
    public class CustomErrorHandler: HandleErrorAttribute
    {
        #region Log Exception
        public override void OnException(ExceptionContext filterContext)
        {
            //Get the exception
            Exception ex = filterContext.Exception;
            string name = System.Web.HttpContext.Current.User.Identity.Name;

            Logger.Instance.LogException(ex,name);

            string message = string.Format("{0} was thrown on the {1}.{4}For: {2}{3}{4}", 
				ex.Message, DateTime.Now, ex.Source, ex.StackTrace, Environment.NewLine);

            //Set the view name to be returned, maybe return different error view for different exception types
            string viewName = "CustomError";

            //Get current controller and action
            string currentController = (string)filterContext.RouteData.Values["controller"];
            string currentActionName = (string)filterContext.RouteData.Values["action"];

            //Create the error model information
            HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, currentController, currentActionName);
            ViewResult result = new ViewResult
            {
                ViewName = viewName,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                TempData = filterContext.Controller.TempData
            };

            filterContext.Result = result;
            filterContext.ExceptionHandled = true;

            // Call the base class implementation:
            base.OnException(filterContext);
        }

        #endregion
    }
}