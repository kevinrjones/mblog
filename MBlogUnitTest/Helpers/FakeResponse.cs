using System.Web;
using Moq;

namespace MBlogUnitTest.Helpers
{
    public class FakeResponse : HttpResponseBase
    {
        // Routing calls this to account for cookieless sessions
        // It's irrelevant for the test, so just return the path unmodified
        private readonly HttpCookieCollection cookies = new HttpCookieCollection();

        public override HttpCookieCollection Cookies
        {
            get { return cookies; }
        }

        public override HttpCachePolicyBase Cache
        {
            get
            {
                var mock = new Mock<HttpCachePolicyBase>();
                return mock.Object;
            }
        }

        public override string ApplyAppPathModifier(string x)
        {
            return x;
        }
    }
}