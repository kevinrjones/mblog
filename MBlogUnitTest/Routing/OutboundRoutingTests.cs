using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MBlog;
using MBlogUnitTest.Helpers;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class OutboundRoutingTests
    {
        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForTheHomePageIndexView_ThenIGetTheCorrectUrl()
        {
            Assert.AreEqual("/", GetOutboundUrl(new
            {
                controller = "Home",
                action = "Index"
            }));
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForABlogPostsPageIndexView_ThenIGetTheCorrectUrl()
        {
            Assert.AreEqual("/nickname", GetOutboundUrl(new
            {
                controller = "Post",
                action = "Index",
                nickname = "nickname"
            }));
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForABlogPostsPageShowView_ThenIGetTheCorrectUrl()
        {
            Assert.AreEqual("/nickname/Show/1999/01/02/link", GetOutboundUrl(new
            {
                controller = "Post",
                action = "Show",
                nickname = "nickname",
                year = "1999",
                month = "01",
                day = "02",
                link = "link"
            }));
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForUserLogout_ThenIGetTheCorrectUrl()
        {
            Assert.AreEqual("/user/logout", GetOutboundUrl(new
            {
                controller = "User",
                action = "logout"
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
    }
}