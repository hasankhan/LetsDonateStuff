using LetsDonateStuff.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using LetsDonateStuff.DAL;
using System.Web;
using System.Web.Security;
using LetsDonateStuff.Models;

namespace LetsDonateStuff.Helpers
{
    public class UserResolver
    {
        ApplicationUserManager userManager;
        IPrincipal principal;
        HttpContextBase context;

        public UserResolver(ApplicationUserManager userManager, IPrincipal principal, HttpContextBase context)
        {
            this.userManager = userManager;
            this.principal = principal;
            this.context = context;
        }

        public ApplicationUser User
        {
            get
            {
                ApplicationUser user = null;
                if (principal.Identity.IsAuthenticated && (user = (ApplicationUser)this.context.Items["user"]) == null)
                    this.context.Items["user"] = user = GetUser();
                return user;
            }
        }

        ApplicationUser GetUser()
        {
            ApplicationUser user = userManager.FindByNameAsync(principal.Identity.Name).Result;
            return user;
        }
    }
}