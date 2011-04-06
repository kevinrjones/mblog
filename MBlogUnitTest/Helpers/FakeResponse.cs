using System.Web;

namespace MBlogUnitTest.Helpers
{
    public class FakeResponse : HttpResponseBase
    {
        // Routing calls this to account for cookieless sessions
        // It's irrelevant for the test, so just return the path unmodified
        private HttpCookieCollection cookies = new HttpCookieCollection();
        public override string ApplyAppPathModifier(string x) { return x; }
        public override HttpCookieCollection Cookies
        {
            get
            {
                return cookies;
            }
        }
    }
}