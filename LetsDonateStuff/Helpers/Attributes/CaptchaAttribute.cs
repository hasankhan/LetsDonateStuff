using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Helpers;
using System.Web.Configuration;
using System.Configuration;

namespace LetsDonateStuff.Helpers.Attributes
{
    public class CaptchaAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isValid = ReCaptcha.Validate(ConfigurationManager.AppSettings["captchaPrivKey"]);
            if (!isValid)
                filterContext.Controller.ViewData.ModelState.AddModelError("ReCaptcha", (string)null);
            base.OnActionExecuting(filterContext);
        }
    }
}