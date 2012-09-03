using System;
using System.Threading;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Infrastructure;
using MBlog.Models.User;
using MBlogModel;
using MBlogServiceInterfaces;

namespace MBlog.Filters
{
    public class GetCookieUserFilterAttribute : AuthorizeAttribute
    {
        public const string UserCookieName = "USER";

        public IUserService UserService { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;
            if (controller != null)
            {
                var userViewModel = new UserViewModel {IsLoggedIn = false};
                if (filterContext.HttpContext.Request.Cookies[UserCookieName] != null)
                {
                    string cookie = filterContext.HttpContext.Request.Cookies[UserCookieName].Value;
                    byte[] cipherText = Convert.FromBase64String(cookie);
                    string plainText = cipherText.Decrypt();
                    int id;
                    if (int.TryParse(plainText, out id))
                    {
                        User user = UserService.GetUserWithTheirBlogs(id);
                        if (user != null)
                        {
                            userViewModel.Id = id;
                            userViewModel.Email = user.Email;
                            userViewModel.Name = user.Name;
                            userViewModel.IsLoggedIn = true;
                            userViewModel.AddNicknamesToUser(user);
                        }
                    }
                }
                filterContext.HttpContext.User = Thread.CurrentPrincipal = userViewModel;
            }
        }
    }
}