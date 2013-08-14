using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LetsDonateStuff.Helpers;

namespace LetsDonateStuff
{
    public class BinderConfig
    {
        public static void RegisterBinders(ModelBinderDictionary binders)
        {
            binders.Add(typeof(MembershipUser), new MembershipUserModelBinder());
        }
    }
}