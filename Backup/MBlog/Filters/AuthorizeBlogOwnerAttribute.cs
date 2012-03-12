using System.Web;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models.User;
using MBlogDomainInterfaces;
using MBlogModel;
using Microsoft.Practices.Unity;

namespace MBlog.Filters
{
    public class AuthorizeBlogOwnerAttribute : AuthorizeLoggedInUserAttribute
    {
        [Dependency]
        public IBlogDomain BlogDomain { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;

            filterContext.HttpContext.Items["controller"] = controller;
            base.OnAuthorization(filterContext);
        }
        
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var handler = httpContext.CurrentHandler as MvcHandler;
            
            var nickname = handler.RequestContext.RouteData.Values["nickname"] as string;
            var blogId = GetBlogId(httpContext, handler);
            var controller = httpContext.Items["controller"] as BaseController;

            if(blogId == null || nickname == null || controller == null)
            {
                var controllerType = controller == null ? "" : controller.GetType().FullName;
                Logger.Error("Authorize failed: blogID: {0}, nickname: {1}, controller: {2}", blogId, nickname, controllerType);
                return false;
            }

            int id = int.Parse(blogId);
            var user = httpContext.User as UserViewModel;
            
            if (!IsLoggedInUser(user) || !UserOwnsBlog(controller, nickname, id))
            {
                Logger.Error("Authorize failed: blogID: {0}, nickname: {1}, user: {2}", blogId, nickname, user);
                return false;
            }
            return true;
        }

        private static string GetBlogId(HttpContextBase httpContext, MvcHandler handler)
        {
            var blogId = handler.RequestContext.RouteData.Values["blogId"] as string;
            if (blogId == null)
            {
                blogId = httpContext.Request["blogId"] as string;
            }
            return blogId;
        }

        private bool UserOwnsBlog(BaseController controller, string nickname, int blogId)
        {            
            Blog blog = BlogDomain.GetBlog(nickname);
            return blog != null && blog.Id == blogId;
        }
    }
}