using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LetsDonateStuff.Controllers
{
    public class ErrorsController: Controller
    {
        public ViewResult NotFound()
        {
            ViewData.Model = Request.Url.PathAndQuery;
            return View();
        }
    }
}