using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MBlog.Controllers.publishing
{
    public class AtomController : Controller
    {
        // GET /api/atom
        public ActionResult GetServiceDocument()
        {
            return View();
        }

    }
}
