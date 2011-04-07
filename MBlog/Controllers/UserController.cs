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
        public UserController(IUserRepository userRepository)
            : base(userRepository) { }

        [HttpGet]
        public ActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            UserViewModel user = HttpContext.User as UserViewModel;
            return RedirectToAction("index", "Post", new { nickname = user.Nickname });
        }

        [HttpPost]
        public ActionResult DoLogin(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Login");
            }
            User user = UserRepository.GetUser(userViewModel.Email);
            if (user != null)
            {
                byte[] cipherText = user.Id.ToString().Encrypt();
                string base64CipherText = Convert.ToBase64String(cipherText);
                Response.Cookies.Add(new HttpCookie(GetCookieUserFilterAttribute.UserCookie, base64CipherText));
                return RedirectToRoute(new { Controller = "Post", action = "Index", nickname = userViewModel.Nickname });
            }
            return View("Login");
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            UserViewModel user = (UserViewModel)HttpContext.User;
            return RedirectToAction("index", "Post", new { nickname = user.Nickname });
        }

        [HttpPost]
        public ActionResult DoRegister(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Register");
            }
            User user = UserRepository.GetUser(userViewModel.Email);
            if (user != null)
            {
                ModelState.AddModelError("EMail", "EMail already exists in database");
                return View("Register");
            }
            user = new User();
            user.AddUser(userViewModel.Name, userViewModel.Email, userViewModel.Password, false);
            UserRepository.Create(user);
            return RedirectToAction("index", "Home");
        }

        public ActionResult Logout()
        {
            HttpCookie cookie;
            if ((cookie = Request.Cookies[GetCookieUserFilterAttribute.UserCookie]) != null)
            {
                Response.Cookies.Remove(cookie.Name);
                HttpContext.User = null;
            }
            return RedirectToRoute(new { Controller = "Home", action = "Index" });
        }
    }
}
