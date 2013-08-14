using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace LetsDonateStuff.Helpers
{
    public static class HtmlHelperExtensions
    {
        static string assemblyVersion;
        
        static HtmlHelperExtensions()
        {
            assemblyVersion = typeof(HtmlHelperExtensions).Assembly.GetName().Version.ToString();
        }

        public static string SiteUrl(this UrlHelper urlHelper)
        {
            string siteUrl = ConfigurationManager.AppSettings["siteUrl"]; ;
            return siteUrl;
        }

        public static string AssmeblyVersion(this HtmlHelper htmlHelper)
        {
            return assemblyVersion;
        }
    }
}