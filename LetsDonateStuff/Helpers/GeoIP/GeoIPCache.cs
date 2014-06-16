using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace LetsDonateStuff.Helpers.GeoIP
{
    public class GeoIPCache
    {
        GeoIPHelper geoIPHelper;
        string cookieName;

        public GeoIPCache(GeoIPHelper geoIPHelper) : this(geoIPHelper, "location") { }

        public GeoIPCache(GeoIPHelper geoIPHelper, string cookieName)
        {
            this.cookieName = cookieName;
            this.geoIPHelper = geoIPHelper;

        }

        public string GetCountry()
        {
            var httpContext = HttpContext.Current;

            string country = null;

            HttpCookie cookie = httpContext.Request.Cookies[cookieName];
            if (cookie == null)
                country = geoIPHelper.GetCountry(httpContext.Request.UserHostAddress);
            else if (!String.IsNullOrEmpty(cookie.Value) && CountryList.IsValidCode(cookie.Value))
                country = cookie.Value;
            
            return country;
        }

        public void SetCountry(string value)
        {
            HttpContext.Current.Response.SetCookie(new HttpCookie(cookieName, value));
        }
    }
}