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
            MaxMind.GeoIP.Country country = this.lookupService.getCountry(ip);
            string code = country.Coalesce(c => c.getCode());
            if (code == null || code == "--")
            {
                return String.Empty;
            }
            return code;
        }

        public PointF GetLocation(string ip)
        {
            var point = new PointF();
            
            return point;
        }
    }
}