using System;
using System.Web;
using System.Web.Mvc;
using MBlog.Filters;
using MBlog.Models;
using MBlog.Models.User;
using MBlogRepository;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    [GetCookieUserFilter]
    public class BaseController : Controller
    {
        protected internal IUserRepository UserRepository { get; set; }
        public IBlogRepository BlogRepository { get; set; }

        public BaseController(IUserRepository userRepository, IBlogRepository blogRepository)
        {
            UserRepository = userRepository;
            BlogRepository = blogRepository;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            // Bail if we can't do anything; app will crash.
            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                if (filterContext == null)
                    return;

                // since we're handling this, log to elmah
                var ex = filterContext.Exception ?? new Exception("No further information exists.");
                //LogException(ex);
                filterContext.ExceptionHandled = true;
                if ((ex.GetType() != typeof (HttpRequestValidationException)))
                {
                    var data = new ErrorPresentation
                                   {
                                       ErrorMessage = HttpUtility.HtmlEncode(ex.Message),
                                       TheException = ex,
                                       ShowMessage = filterContext.Exception != null,
                                       ShowLink = false
                                   };
                    filterContext.Result = View("Error", data);
                }
            }
        }

        protected bool IsLoggedInUser(UserViewModel user)
        {
            return (user != null && user.IsLoggedIn);
        }

        private bool UserOwnsBlog(string nickname, int blogId)
        {
            var blog = BlogRepository.GetBlog(nickname);
            return blog.Id == blogId;
        }

        protected bool RedirectIfInvalidUser(string nickname, int blogId, out ActionResult redirectToAction)
        {
            var user = HttpContext.User as UserViewModel;
            if (!IsLoggedInUser(user) || !UserOwnsBlog(nickname, blogId))
            {
                redirectToAction = RedirectToAction("login", "user");
                return true;
            }
            redirectToAction = null;
            return false;
        }
    }

    public class ErrorPresentation
    {
        public string ErrorMessage;
        public Exception TheException;
        public bool ShowMessage;
        public bool ShowLink;
    }
}