using System;
using System.Web;
using System.Web.Mvc;
using MBlog.Filters;
using MBlog.Infrastructure;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUsernameBlacklistRepository _usernameBlacklistRepository;

        public UserController(IUserRepository userRepository, IUsernameBlacklistRepository usernameBlacklistRepository,
                              IBlogRepository blogRepository)
            : base(userRepository, blogRepository)
        {
            _usernameBlacklistRepository = usernameBlacklistRepository;
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
            User user = UserRepository.GetUser(userViewModel.Email);
            if (!IsRegistrationValid(userViewModel, user))
            {
                return View("Register");
            }

            user = new User(userViewModel.Name, userViewModel.Email, userViewModel.Password, false);
            UserRepository.Create(user);
            UpdateCookiesAndContext(user);
            return RedirectToAction("index", "Dashboard");
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