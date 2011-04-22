using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBlog.Models;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(IUserRepository userRepository) : base(userRepository)
        {}

        public ActionResult Index()
        {
            UserViewModel user = HttpContext.User as UserViewModel;
            if (user != null)
                return View(user);

            return RedirectToAction("index", "home");
        }
    }
}
