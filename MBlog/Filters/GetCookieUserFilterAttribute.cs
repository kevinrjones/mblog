using System;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Infrastructure;
using MBlog.Models.User;
using MBlogModel;

namespace MBlog.Filters
{
    public class GetCookieUserFilterAttribute : AuthorizeAttribute
    {
        public static string UserCookie = "USER";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {            
            var controller = filterContext.Controller as BaseController;
            if (controller != null)
            {
                var userViewModel = new UserViewModel { IsLoggedIn = false };
                if (filterContext.HttpContext.Request.Cookies[UserCookie] != null)
                {
                    string cookie = filterContext.HttpContext.Request.Cookies[UserCookie].Value;
                    byte[] cipherText = Convert.FromBase64String(cookie);
                    string plainText = cipherText.Decrypt();
                    int id;
                    if (int.TryParse(plainText, out id))
                    {
                        User user = controller.UserRepository.GetUserWithTheirBlogs(id);
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
                filterContext.HttpContext.User = userViewModel;
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