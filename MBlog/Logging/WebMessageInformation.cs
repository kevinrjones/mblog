using System.Web;

namespace MBlog.Logging
{
    public class WebMessageInformation : IMessageInformation
    {
        public string Path
        {
            get { return HttpContext.Current.Request.Path; }
            set { throw new System.NotImplementedException(); }
        }

        public string RawUrl
        {
            get { return HttpContext.Current.Request.RawUrl; }
            set { throw new System.NotImplementedException(); }
        }
    }
}