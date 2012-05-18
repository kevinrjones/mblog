﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Logging;
using MBlog.Filters;
using MBlog.Infrastructure;
using MBlog.Models.User;
using MBlogModel;
using MBlogServiceInterfaces;

namespace MBlog.Controllers
{
    public class SessionController : BaseController
    {
        private readonly IUserService _userService;

        public SessionController(IUserService userService, ILogger logger) : base(logger)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult New()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {                
                return View();
            }
            return RedirectToAction("index", "Dashboard");
        }

        [HttpPost]
        public ActionResult Create(LoginUserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("New");
            }
            User user = _userService.GetUser(userViewModel.Email);
            if (user != null && user.MatchPassword(userViewModel.Password))
            {
                UpdateCookiesAndContext(user);
                return RedirectToRoute(new {action = "Index", controller = "Dashboard"});
            }
            return View("New");
        }

        [HttpGet]
        public ActionResult Delete()
        {
            HttpCookie cookie;
            if ((cookie = Request.Cookies[GetCookieUserFilterAttribute.UserCookie]) != null)
            {
                cookie.Expires = new DateTime(1970, 1, 1);
                Response.Cookies.Add(cookie);
                HttpContext.User = null;
            }
            return RedirectToAction("index", "Home");
        }

        private void UpdateCookiesAndContext(User user)
        {
            byte[] cipherText = user.Id.ToString().Encrypt();
            string base64CipherText = Convert.ToBase64String(cipherText);
            Response.Cookies.Add(new HttpCookie(GetCookieUserFilterAttribute.UserCookie, base64CipherText));
            HttpContext.User = new UserViewModel {Email = user.Email, Name = user.Name, IsLoggedIn = true};
        }
    }
}