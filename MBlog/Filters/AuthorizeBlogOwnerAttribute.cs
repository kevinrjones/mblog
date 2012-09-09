using System;
using System.Web;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models.User;
using MBlogServiceInterfaces;

namespace MBlog.Filters
{
    public class AuthorizeBlogOwnerAttribute : AuthorizeLoggedInUserAttribute
    {
        public IBlogService BlogService { get; set; }
        public IUserService UserService { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;

            filterContext.HttpContext.Items["controller"] = controller;
            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var handler = httpContext.CurrentHandler as MvcHandler;
            if(handler == null)
            {
                throw new NullReferenceException("Must be an instance of an MvcHandler");
            }

            var nickname = handler.RequestContext.RouteData.Values["nickname"] as string;
            var controller = httpContext.Items["controller"] as BaseController;

            if (nickname == null || controller == null)
            {
                string controllerType = controller == null ? "" : controller.GetType().FullName;
                Logger.Error("Authorize failed: blogID: nickname: {0}, controller: {1}", nickname,
                             controllerType);
                return false;
            }

            var user = httpContext.User as UserViewModel;

            if (!IsLoggedInUser(user) || !UserOwnsBlog(user, nickname))
            {
                Logger.Error("Authorize failed: for nickname: {0}, user: {1}", nickname, user.Email, user.AuthenticationType);
                return false;
            }
            return true;
        }

        private bool UserOwnsBlog(UserViewModel sessionUser, string nickname)
        {
            var user = UserService.GetUser(sessionUser.Email);
            var blog = BlogService.GetBlog(nickname);
            return blog != null && user!= null && blog.UserId == user.Id;
        }
    }
}