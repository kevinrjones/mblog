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

                strErrorMsg += "Raw Url: " + messageInformation.RawUrl + Environment.NewLine;
            }

            strErrorMsg += "Message: " + exception.Message + Environment.NewLine;

            strErrorMsg += "Source: " + exception.Source + Environment.NewLine;

            strErrorMsg += "Stack Trace: " + exception.StackTrace + Environment.NewLine;

            strErrorMsg += "Target Site: " + exception.TargetSite;
            return strErrorMsg;
        }
    }
}
