using System;
using System.Web;

namespace MBlog.Logging
{
    public static class ExceptionFormatter
    {
        public static string BuildExceptionMessage(this Exception exception, HttpContext httpContext)
        {            
            string strErrorMsg = Environment.NewLine + "Error in Path :" + httpContext.Request.Path;

            // Get the QueryString along with the Virtual Path
            strErrorMsg += Environment.NewLine + "Raw Url :" + httpContext.Request.RawUrl;

            // Get the error message
            strErrorMsg += Environment.NewLine + "Message :" + exception.Message;

            // Source of the message
            strErrorMsg += Environment.NewLine + "Source :" + exception.Source;

            // Stack Trace of the error

            strErrorMsg += Environment.NewLine + "Stack Trace :" + exception.StackTrace;

            // Method where the error occurred
            strErrorMsg += Environment.NewLine + "TargetSite :" + exception.TargetSite;
            return strErrorMsg;
        }
    }
}
