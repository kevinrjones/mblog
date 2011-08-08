using System.Web.Mvc;
using Logging;
using MBlog.Controllers;
using MBlog.Models.User;
using Microsoft.Practices.Unity;

namespace MBlog.Filters
{
    public class AuthorizeLoggedInUserAttribute : AuthorizeAttribute
    {
        [Dependency]
        public ILogger Logger { get; set; }

        public AuthorizeLoggedInUserAttribute()
        {
            Logger = new NullLogger();
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/user/login");
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var user = httpContext.User as UserViewModel;

            if (!IsLoggedInUser(user))
            {
                return false;
            }
            return true;
        }

        protected bool IsLoggedInUser(UserViewModel user)
        {
            return (user != null && user.IsLoggedIn);
        }
    }
}