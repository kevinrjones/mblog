using System.Web;
using System.Web.Mvc;
using Logging;

namespace MBlog.Infrastructure.Logging.NLog
{
    public class NLogHandleErrorAttribute : HandleErrorAttribute
    {
        private readonly ILogger _logger;

        public NLogHandleErrorAttribute(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext filterContext)
        {            
            _logger.Error(filterContext.Exception, HttpContext.Current.Request.Path, HttpContext.Current.Request.RawUrl);
            base.OnException(filterContext);
        }
    }
}