using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MBlog.Filters;
using MBlog.Infrastructure;
using MBlog.Models;
using MBlogModel;
using MBlogRepository;
using MBlog.Infrastructure;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUsernameBlacklistRepository _usernameBlacklistRepository;

        public UserController(IUserRepository userRepository, IUsernameBlacklistRepository usernameBlacklistRepository)
            : base(userRepository)
        {
            _usernameBlacklistRepository = usernameBlacklistRepository;
        }

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
            if (user != null && user.MatchPassword(userViewModel.Password))
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
            User user = UserRepository.GetUser(userViewModel.Email);
            if(!IsRegistrationValid(userViewModel, user))
            {
                return View("Register");
                
            }
            
            user = new User();
            user.AddUserDetails(userViewModel.Name, userViewModel.Email, userViewModel.Password, false);
            UserRepository.Create(user);
            return RedirectToAction("index", "Home");
        }

        private bool IsRegistrationValid(UserViewModel userViewModel, User user)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }
            if (user != null)
            {
                ModelState.AddModelError("EMail", "EMail already exists in database");
                return false;
            }
            Blacklist blacklist = _usernameBlacklistRepository.GetName(userViewModel.Name);
            if (blacklist != null)
            {
                ModelState.AddModelError("Name", "That user name is reserved");
                return false;
            }
            return true;
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
