using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LetsDonateStuff.DAL;
using System.Data.Entity;
using MaxMind.GeoIP;
using LetsDonateStuff.Helpers;
using LetsDonateStuff.Controllers;
using System.Diagnostics;
using System.Net;
using LetsDonateStuff.Services;
using System.Web.Security;
using System.Web.Http;
using System.Web.Optimization;
using WebMatrix.WebData;
using System.Data.Entity.Infrastructure;

namespace LetsDonateStuff
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            BinderConfig.RegisterBinders(ModelBinders.Binders);
            FilterConfig.RegisterFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Bootstrapper.Initialise();
        }        

        protected void Application_Error()
        {
            var error = Server.GetLastError();
            var code = (error as HttpException).Coalesce(e=>e.GetHttpCode(), (int)HttpStatusCode.InternalServerError);

            if (code == (int)HttpStatusCode.NotFound)
            {
                Response.Clear();

                var routeData = new RouteData();
                routeData.Values["controller"] = "Errors";
                routeData.Values["action"] = "NotFound";

                IController controller = new ErrorsController();
                controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));

                Response.StatusCode = (int)HttpStatusCode.NotFound;
                Response.End();
            }
        }
    }
}