using System;
using System.Web;
using System.Web.Mvc;
using MBlog.Filters;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    [GetCookieUserFilter]
    public class BaseController : Controller
    {
        public BaseController(IUserRepository userRepository, IBlogRepository blogRepository)
        {
            UserRepository = userRepository;
            BlogRepository = blogRepository;
        }

        protected internal IUserRepository UserRepository { get; set; }
        public IBlogRepository BlogRepository { get; set; }

        protected override void OnException(ExceptionContext filterContext)
        {
            // Bail if we can't do anything; app will crash.
            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                if (filterContext == null)
                    return;

                // since we're handling this, log to elmah
                Exception ex = filterContext.Exception ?? new Exception("No further information exists.");
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
            Blog blog = BlogRepository.GetBlog(nickname);
            return blog.Id == blogId;
        }

        protected bool RedirectIfInvalidUser(string nickname, int blogId, out ActionResult redirectToAction)
        {
            var user = HttpContext.User as UserViewModel;
            if (!IsLoggedInUser(user) || !UserOwnsBlog(nickname, blogId))
            {
                redirectToAction = RedirectToAction("login", "User");
                return true;
            }
            redirectToAction = null;
            return false;
        }
    }

    public class ErrorPresentation
    {
        public string ErrorMessage;
        public bool ShowLink;
        public bool ShowMessage;
        public Exception TheException;
    }
}