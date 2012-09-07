using System;
using System.Web;
using System.Web.Mvc;
using Elmah;
using Logging;
using MBlog.Filters;
using MBlog.Logging;
using MBlog.Models.Error;
using MBlog.Models.User;

namespace MBlog.Controllers
{
    [GetCookieUserFilter]
    public partial class BaseController : Controller
    {
        public BaseController(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; set; }

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
            HttpContext context = System.Web.HttpContext.Current;
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
    }
}