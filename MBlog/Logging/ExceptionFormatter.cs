using System;
using System.Web;

namespace MBlog.Logging
{
    public static class ExceptionFormatter
    {
        public static string BuildExceptionMessage(this Exception exception, IMessageInformation messageInformation)
        {
            string strErrorMsg = "";
            if (messageInformation != null)
            {
                strErrorMsg += "Error in Path: " + messageInformation.Path + Environment.NewLine;

                // Get the QueryString along with the Virtual Path
                strErrorMsg += "Raw Url: " + messageInformation.RawUrl + Environment.NewLine;
            }
            // Get the error message
            strErrorMsg += "Message: " + exception.Message + Environment.NewLine;

            // Source of the message
            strErrorMsg += "Source: " + exception.Source + Environment.NewLine;

            // Stack Trace of the error

            strErrorMsg += "Stack Trace: " + exception.StackTrace + Environment.NewLine;

            // Method where the error occurred
            strErrorMsg += "Target Site: " + exception.TargetSite;
            return strErrorMsg;
        }
    }
}
