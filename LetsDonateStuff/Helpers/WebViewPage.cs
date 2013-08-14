using LetsDonateStuff.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LetsDonateStuff.Helpers
{
    public abstract class WebViewPage<TModel>: System.Web.Mvc.WebViewPage<TModel>
    {
        UserResolver userResolver;

        protected WebViewPage ()
	    {
            userResolver = DependencyResolver.Current.GetService<UserResolver>();
	    }
    
        protected MembershipUser GetUser()
        {
            return userResolver.User;
        }
    }
}