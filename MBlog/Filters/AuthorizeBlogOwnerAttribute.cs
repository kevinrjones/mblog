using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Filters
{
    public class AuthorizeBlogOwnerAttribute : AuthorizeLoggedInUserAttribute
    {
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
            var blogId = handler.RequestContext.RouteData.Values["blogId"] as string;
            var controller = httpContext.Items["controller"] as BaseController;

            if(blogId == null || nickname == null || controller == null)
            {
                return false;
            }

            int id = int.Parse(blogId);
            var user = httpContext.User as UserViewModel;
            
            if (!IsLoggedInUser(user) || !UserOwnsBlog(controller, nickname, id))
            {
                return false;
            }
            return true;
        }

        private bool UserOwnsBlog(BaseController controller, string nickname, int blogId)
        {
            IBlogRepository blogRepository = controller.BlogRepository;
            Blog blog = blogRepository.GetBlog(nickname);
            return blog != null && blog.Id == blogId;
        }
    }
}