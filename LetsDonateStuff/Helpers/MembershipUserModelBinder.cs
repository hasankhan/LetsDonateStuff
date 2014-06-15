using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LetsDonateStuff.Models;

namespace LetsDonateStuff.Helpers
{
    public class MembershipUserModelBinder: IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ApplicationUser user = DependencyResolver.Current.GetService<UserResolver>().User;
            return user;
        }
    }
}