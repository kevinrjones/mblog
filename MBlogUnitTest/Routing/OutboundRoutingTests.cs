using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MBlog;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class OutboundRoutingTests
    {
        [Test]
        public void All_Products_Page_1_Is_At_Slash()
        {
            Assert.AreEqual("/", GetOutboundUrl(new
            {
                controller = "Products",
                action = "List",
                category = (string)null,
                page = 1
            }));
        }

        string GetOutboundUrl(object routeValues)
        {
            // Get route configuration and mock request context
            RouteCollection routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            var mockHttpContext = new Mock<HttpContextBase>();
            var mockRequest = new Mock<HttpRequestBase>();
            var fakeResponse = new FakeResponse();
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(x => x.Response).Returns(fakeResponse);
            mockRequest.Setup(x => x.ApplicationPath).Returns("/");

            // Generate the outbound URL
            var ctx = new RequestContext(mockHttpContext.Object, new RouteData());
            return routes.GetVirtualPath(ctx, new RouteValueDictionary(routeValues))
                .VirtualPath;
        }

        private class FakeResponse : HttpResponseBase
        {
            // Routing calls this to account for cookieless sessions
            // It's irrelevant for the test, so just return the path unmodified
            public override string ApplyAppPathModifier(string x) { return x; }
        }

    }
}