using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MBlog.Controllers.publishing
{
    public partial class RsdController : Controller
    {
        //
        // GET: /Rsd/

        public virtual ActionResult Rsd()
        {
            return View();
        }

        public string Atom()
        {
            return "dd";
        }
    }
}
