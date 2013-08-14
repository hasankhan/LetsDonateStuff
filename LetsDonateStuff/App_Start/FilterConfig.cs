using LetsDonateStuff.DAL;
using LetsDonateStuff.Filters;
using LetsDonateStuff.Services;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LetsDonateStuff
{
    public class FilterConfig
    {
        public static void RegisterFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LetsDonateStuff.Helpers.Attributes.HandleErrorAttribute());
        }
    }
}