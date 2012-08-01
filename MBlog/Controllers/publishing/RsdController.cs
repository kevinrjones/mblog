using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MBlog.Controllers.publishing
{
    public class RsdController : Controller
    {
        //
        // GET: /Rsd/

        public ActionResult Rsd()
        {
            return View();
        }

        public string Atom()
        {
            return "dd";
        }
    }
}
