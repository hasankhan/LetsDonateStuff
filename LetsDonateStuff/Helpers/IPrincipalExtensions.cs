using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using LetsDonateStuff.DAL;

namespace LetsDonateStuff.Helpers
{
    public static class IPrincipalExtensions
    {
        public static bool IsAdmin(this IPrincipal user)
        {
            return user.IsInRole(UserRoles.Admin);
        }

        public static bool IsModerator(this IPrincipal user)
        {
            return user.IsInRole(UserRoles.Moderator) || IsAdmin(user);
        }
    }
}