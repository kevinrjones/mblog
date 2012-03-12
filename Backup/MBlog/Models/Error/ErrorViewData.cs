using System;

namespace MBlog.Models.Error
{
    public class ErrorViewData
    {
        public string ErrorMessage;
        public bool ShowLink;
        public bool ShowMessage;
        public Exception TheException;
    }
}