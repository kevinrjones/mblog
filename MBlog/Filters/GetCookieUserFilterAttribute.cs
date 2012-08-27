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
        public const string UserCookie = "USER";

        public IUserService UserService { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;
            if (controller != null)
            {
                var userViewModel = new UserViewModel {IsLoggedIn = false};
                if (filterContext.HttpContext.Request.Cookies[UserCookie] != null)
                {
                    string cookie = filterContext.HttpContext.Request.Cookies[UserCookie].Value;
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
                            AddNicknamesToUser(user, userViewModel);
                        }
                    }
                }
                filterContext.HttpContext.User = Thread.CurrentPrincipal = userViewModel;
            }
        }

        private void AddNicknamesToUser(User user, UserViewModel userViewModel)
        {
            foreach (Blog blog in user.Blogs)
            {
                userViewModel.Nicknames.Add(blog.Nickname);
            }
        }
    }
}