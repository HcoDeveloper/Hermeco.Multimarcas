﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;


namespace Hermeco.Multimarcas
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception TheError = Server.GetLastError();
            Server.ClearError();

            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;

            if (TheError is HttpException && ((HttpException)TheError).GetHttpCode() == 404)
            {
                Response.Redirect("~/");
            }
            else
            {
                Response.Redirect("~/");
            }
        }
    }
}
