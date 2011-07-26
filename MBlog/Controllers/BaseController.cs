using System;
using System.Web;
using System.Web.Mvc;
using Elmah;
using Logging;
using MBlog.Filters;
using MBlog.Models.Error;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository.Interfaces;
using MBlog.Logging;

namespace MBlog.Controllers
{
    [GetCookieUserFilter]
    public class BaseController : Controller
    {
        protected ILogger Logger { get; set; }
        protected internal IUserRepository UserRepository { get; set; }
        protected IBlogRepository BlogRepository { get; set; }

        public BaseController(ILogger logger, IUserRepository userRepository, IBlogRepository blogRepository)
        {
            Logger = logger;
            UserRepository = userRepository;
            BlogRepository = blogRepository;
        }

        public BaseController(IUserRepository userRepository, IBlogRepository blogRepository) : this (null, userRepository, blogRepository)
        {
            UserRepository = userRepository;
            BlogRepository = blogRepository;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            // Bail if we can't do anything; app will crash.
            if (filterContext == null)
                return;

            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                Exception ex = filterContext.Exception ?? new Exception("No further information exists.");
                LogExceptionToElmah(ex);
                filterContext.ExceptionHandled = true;
                if ((ex.GetType() != typeof (HttpRequestValidationException)))
                {
                    var data = new ErrorViewData
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

        private void LogExceptionToElmah(Exception exception)
        {
            var context = System.Web.HttpContext.Current;
            ErrorLog.GetDefault(context).Log(new Error(exception, context));
            Exception logException = exception;
            while (logException != null)
            {
                Logger.Error(exception.BuildExceptionMessage(new WebMessageInformation()), logException);
                logException = exception.InnerException;
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
}