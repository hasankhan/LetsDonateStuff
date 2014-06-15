using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MaxMind.GeoIP;
using System.Drawing;

namespace LetsDonateStuff.Helpers.GeoIP
{
    public class GeoIPHelper
    {
        LookupService lookupService;

        public GeoIPHelper()
        {
            string dbPath = HttpContext.Current.Server.MapPath(@"~\App_Data\GeoLiteCity.dat");
            this.lookupService = new LookupService(dbPath, LookupService.GEOIP_MEMORY_CACHE);
        }

        public string GetCountry(string ip)
        {
            return String.Empty;
        }

        public PointF GetLocation(string ip)
        {
            var point = new PointF();
            
            return point;
        }
    }
}