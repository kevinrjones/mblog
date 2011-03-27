using System;
using System.Web;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Infrastructure;
using MBlog.Models;
using MBlogModel;

namespace MBlog.Filters
{
    public class GetCookieUserFilterAttribute : ActionFilterAttribute
    {
        public static string UserCookie = "USER";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            BaseController controller = filterContext.Controller as BaseController;
            if (controller != null)
            {
                UserViewModel userViewModel = new UserViewModel { IsLoggedIn = false };
                if (HttpContext.Current.Request.Cookies[UserCookie] != null)
                {
                    string cookie = HttpContext.Current.Request.Cookies[UserCookie].Value;
                    var cipherText = Convert.FromBase64String(cookie);
                    string plainText = cipherText.Decrypt();
                    int id;
                    if (int.TryParse(plainText, out id))
                    {
                        User user = controller.UserRepository.GetUser(id);
                        if (user != null)
                        {
                            userViewModel.Id = id;
                            userViewModel.Email = user.Email;
                            userViewModel.Name = user.Name;
                            userViewModel.IsLoggedIn = true;
                        }
                    }
                }
                HttpContext.Current.User = userViewModel;
            }
        }
    }
}