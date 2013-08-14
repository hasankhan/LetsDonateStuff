using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace LetsDonateStuff.Services
{
    public class UserToken
    {
        static UserToken admin;

        static UserToken()
        {
            admin = new UserToken(new GenericPrincipal(new GenericIdentity("Hasan"), new[] { "Admin" }));
        }

        public string Username { get; private set; }
        public IPrincipal Principal { get; private set; }

        public UserToken(IPrincipal principal)
        {
            this.Principal = principal;
            this.Username = principal.Identity.IsAuthenticated ? principal.Identity.Name : "Anonymous";
        }

        public static UserToken Admin
        {
            get { return admin; }
        }
    }
}