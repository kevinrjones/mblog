using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Logging;
using MBlog.Filters;
using MBlog.Infrastructure;
using MBlog.Models.User;
using MBlogModel;
using MBlogServiceInterfaces;
using MBlogServiceInterfaces.ModelState;

namespace MBlog.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, ILogger logger)
            : base(logger)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult New()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Register");
            }
            return RedirectToAction("index", "Dashboard");
        }

        [HttpPost]
        public ActionResult Create(UserViewModel userViewModel)
        {
            List<ErrorDetails> errorDetails;
            if (!ModelState.IsValid)
            {
                return View("Register");
            }
            errorDetails = _userService.IsUserRegistrationValid(userViewModel.Name, userViewModel.Email);
            if (errorDetails.Count != 0)
            {
                foreach (ErrorDetails errorDetail in errorDetails)
                {
                    ModelState.AddModelError(errorDetail.FieldName, errorDetail.Message);
                }
                return View("Register");
            }

            User user = _userService.CreateUser(userViewModel.Name, userViewModel.Email, userViewModel.Password);
            UpdateCookiesAndContext(user);
            return RedirectToAction("index", "Dashboard");
        }

        public ActionResult Logout()
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