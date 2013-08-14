using LetsDonateStuff.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using LetsDonateStuff.DAL;
using System.Web;
using System.Web.Security;

namespace LetsDonateStuff.Helpers
{
    public class UserResolver
    {
        IMembershipService membershipService;
        IPrincipal principal;
        HttpContextBase context;

        public UserResolver(IMembershipService membershipService, IPrincipal principal, HttpContextBase context)
        {
            this.membershipService = membershipService;
            this.principal = principal;
            this.context = context;
        }

        public MembershipUser User
        {
            get
            {
                MembershipUser user = null;
                if (principal.Identity.IsAuthenticated && (user = (MembershipUser)this.context.Items["user"]) == null)
                    this.context.Items["user"] = user = GetUser();
                return user;
            }
        }

        MembershipUser GetUser()
        {
            MembershipUser user = membershipService.GetUser(principal.Identity.Name);
            return user;
        }
    }
}