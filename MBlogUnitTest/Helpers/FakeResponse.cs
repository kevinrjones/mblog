using System.Collections.Specialized;
using System.Web;
using Moq;

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

        public override HttpCachePolicyBase Cache
        {
            get
            {
                var mock = new Mock<HttpCachePolicyBase>();
                return mock.Object;
            }
        }
    }

    public class FakeRequest : HttpRequestBase
    {
        readonly NameValueCollection _values = new NameValueCollection();

        public FakeRequest()
        {
            _values.Add("blogId", "1");
        }

        public override string this[string key]
        {
            get
            {
                return _values[key];
            }
        }
    }
}