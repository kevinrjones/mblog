using System.Web;
using System.Web.Mvc;
using Logging;
using MBlog.Models.User;
using Microsoft.Practices.Unity;

namespace MBlog.Filters
{
    public class AuthorizeLoggedInUserAttribute : AuthorizeAttribute
    {
        public AuthorizeLoggedInUserAttribute()
        {
            Logger = new NullLogger();
        }

        [Dependency]
        public ILogger Logger { get; set; }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/session/new");
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
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