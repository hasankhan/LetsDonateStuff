using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LetsDonateStuff.Helpers;
using LetsDonateStuff.Models;

namespace LetsDonateStuff
{
    public class BinderConfig
    {
        public static void RegisterBinders(ModelBinderDictionary binders)
        {
            binders.Add(typeof(ApplicationUser), new MembershipUserModelBinder());
        }
    }
}