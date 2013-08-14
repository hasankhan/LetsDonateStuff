using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LetsDonateStuff.Helpers
{
    public class MembershipUserModelBinder: IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            MembershipUser user = DependencyResolver.Current.GetService<UserResolver>().User;
            return user;
        }
    }
}