using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBlogRepository;

namespace MBlog.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IUserRepository userRepository) : base(userRepository)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
