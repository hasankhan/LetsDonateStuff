using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaxMind.GeoIP;
using System.Threading;
using System.Web.Routing;
using LetsDonateStuff.Helpers.GeoIP;

namespace LetsDonateStuff.Helpers.Attributes
{
    public class GeoIPCountryAttribute : ActionFilterAttribute
    {
        static Lazy<GeoIPHelper> geoIPHelper;
        string paramName;
        string bindTo;

        static GeoIPCountryAttribute()
        {
            geoIPHelper = new Lazy<GeoIPHelper>();
        }

        public GeoIPCountryAttribute(string paramName): this(paramName, null) {}

        public GeoIPCountryAttribute(string paramName, string bindTo)
        {
            this.paramName = paramName;
            this.bindTo = bindTo;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.HttpContext.User.IsModerator())
                return;

            var geoIPCache = new GeoIPCache(geoIPHelper.Value);

            string country = (string)filterContext.ActionParameters[paramName];

            if (country == null)
            {
                country = geoIPCache.GetCountry();
                SetCountryParam(filterContext, country);
            }

            geoIPCache.SetCountry(country);
        }

        void SetCountryParam(ActionExecutingContext filterContext, string country)
        {
            if (String.IsNullOrEmpty(country))
                return;

            var routeValues = new RouteValueDictionary();
            routeValues.Add("action", "Index");
            routeValues.Add("controller", "Post");

            foreach (string key in filterContext.Controller.ViewData.ModelState.Keys)
                routeValues[key] = filterContext.Controller.ViewData.ModelState[key].Value.AttemptedValue;

            routeValues[bindTo ?? paramName] = country;

            filterContext.Result = new RedirectToRouteResult(routeValues);
        }
    }
}