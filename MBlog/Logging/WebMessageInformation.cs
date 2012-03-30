using System;
using System.Web;

namespace MBlog.Logging
{
    public class WebMessageInformation : IMessageInformation
    {
        #region IMessageInformation Members

        public string Path
        {
            get { return HttpContext.Current.Request.Path; }
            set { throw new NotImplementedException(); }
        }

        public string RawUrl
        {
            get { return HttpContext.Current.Request.RawUrl; }
            set { throw new NotImplementedException(); }
        }

        #endregion
    }
}