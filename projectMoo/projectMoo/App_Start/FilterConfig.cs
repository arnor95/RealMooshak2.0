﻿using projectMoo.Handlers;
using System.Web;
using System.Web.Mvc;

namespace projectMoo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomErrorHandler());
            //filters.Add(new HandleErrorAttribute());
        }
    }
}
