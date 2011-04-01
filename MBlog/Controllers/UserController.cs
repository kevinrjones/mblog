using System;
using System.Web;
using System.Web.Mvc;
using MBlog.Filters;
using MBlog.Infrastructure;
using MBlog.Models;
using MBlogModel;
using MBlogRepository;

namespace MBlog.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;


        public UserController(IUserRepository userRepository)
            : base(userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("index", "Home");
        }

        [HttpPost]
        public ActionResult DoRegister(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Register");
            }
            //UserRepository.Create(new User{Email = userViewModel.Email});
            return RedirectToAction("index", "Home");
        }

        [HttpPost]
        public ActionResult DoLogin(UserViewModel userViewModel)
        {
            User user = _userRepository.GetUser(userViewModel.Email);
            if (user != null)
            {
                byte[] cipherText = user.Id.ToString().Encrypt();
                string base64CipherText = Convert.ToBase64String(cipherText);
                Response.Cookies.Add(new HttpCookie(GetCookieUserFilterAttribute.UserCookie, base64CipherText));
            }
            return RedirectToRoute(new { Controller = "questions", action = "Index" });
        }

        public ActionResult Logout()
        {
            if (Request.Cookies[GetCookieUserFilterAttribute.UserCookie] != null)
            {
                HttpCookie myCookie = new HttpCookie(GetCookieUserFilterAttribute.UserCookie);
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
            return RedirectToRoute(new { Controller = "questions", action = "Index" });
        }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}
