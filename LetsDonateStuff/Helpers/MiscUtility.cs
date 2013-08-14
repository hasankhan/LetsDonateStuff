using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace LetsDonateStuff.Helpers
{
    public class MiscUtility
    {
        public static IEnumerable<string> GetHubUrls()
        {
            string feedHubs = ConfigurationManager.AppSettings["feedHubs"];
            IEnumerable<string> hubUrls = feedHubs.Split(',').Select(s => s.Trim());

            return hubUrls;
        }
    }
}