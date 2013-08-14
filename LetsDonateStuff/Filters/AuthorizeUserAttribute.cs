using LetsDonateStuff.DAL;
using LetsDonateStuff.Helpers;
using LetsDonateStuff.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LetsDonateStuff.Filters
{
    public class AuthorizeUserAttribute: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorized = base.AuthorizeCore(httpContext);

            if (authorized)
                authorized = GetUser() != null;

            return authorized;
        }

        MembershipUser GetUser()
        {
            MembershipUser user = DependencyResolver.Current.GetService<UserResolver>().User;
            return user;
        }
    }
}