using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MBlog;
using MBlogUnitTest.Helpers;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Routing
{
    [TestFixture]
    public class OutboundRoutingTests
    {
        private string GetOutboundUrl(object routeValues)
        {
            // Get route configuration and mock request context
            var routes = new RouteCollection();
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

        private static UrlHelper GetUrlHelper(string appPath = "/", RouteCollection routes = null)
        {
            if (routes == null)
            {
                routes = new RouteCollection();
                MvcApplication.RegisterRoutes(routes);
            }

            HttpContextBase httpContext = new StubHttpContextForRouting(appPath);
            var routeData = new RouteData();
            routeData.Values.Add("controller", "defaultcontroller");
            routeData.Values.Add("action", "defaultaction");
            var requestContext = new RequestContext(httpContext, routeData);
            var helper = new UrlHelper(requestContext, routes);
            return helper;
        }

        public class StubHttpContextForRouting : HttpContextBase
        {
            private readonly StubHttpRequestForRouting _request;
            private readonly StubHttpResponseForRouting _response;

            public StubHttpContextForRouting(string appPath = "/", string requestUrl = "~/")
            {
                _request = new StubHttpRequestForRouting(appPath, requestUrl);
                _response = new StubHttpResponseForRouting();
            }

            public override HttpRequestBase Request
            {
                get { return _request; }
            }

            public override HttpResponseBase Response
            {
                get { return _response; }
            }
        }

        public class StubHttpRequestForRouting : HttpRequestBase
        {
            private readonly string _appPath;
            private readonly string _requestUrl;

            public StubHttpRequestForRouting(string appPath, string requestUrl)
            {
                _appPath = appPath;
                _requestUrl = requestUrl;
            }

            public override string ApplicationPath
            {
                get { return _appPath; }
            }

            public override string AppRelativeCurrentExecutionFilePath
            {
                get { return _requestUrl; }
            }

            public override string PathInfo
            {
                get { return ""; }
            }

            public override NameValueCollection ServerVariables
            {
                get { return new NameValueCollection(); }
            }
        }

        public class StubHttpResponseForRouting : HttpResponseBase
        {
            public override string ApplyAppPathModifier(string virtualPath)
            {
                return virtualPath;
            }
        }

        [Test]
        public void ActionWithSpecificControllerAndAction()
        {
            UrlHelper helper = GetUrlHelper();

            string url = helper.Action("index", "home");

            Assert.AreEqual("/", url);
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
            /*@Html.ActionLink(Model.Title, "show", new { controller = "Post", link = Model.Link, year = Model.YearPosted, month = Model.MonthPosted, day = Model.DayPosted })*/
            UrlHelper helper = GetUrlHelper();

            string expectedurl = "/nickname/1999/01/02/link";

            string year = 1999.ToString("D4");
            string month = 1.ToString("D2");
            string day = 2.ToString("D2");
            string url = helper.Action("Show", "Post", new {nickname = "nickname", year, month, day, link = "link"});

            Assert.AreEqual(expectedurl, url);
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForANewBlogPost_ThenIGetTheCorrectUrl()
        {
            /*@Html.ActionLink("Foo", "New", "Post")*/
            UrlHelper helper = GetUrlHelper();

            string expectedurl = "/nickname/new/1";

            string url = helper.Action("New", "Post", new {nickname = "nickname", blogId = 1});

            Assert.AreEqual(expectedurl, url);
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForCreatingABlogPost_ThenIGetTheCorrectUrl()
        {
            /*@Html.ActionLink("Foo", "Create", "Post")*/
            UrlHelper helper = GetUrlHelper();

            string expectedurl = "/nickname/create";

            string url = helper.Action("Create", "Post", new {nickname = "nickname"});

            Assert.AreEqual(expectedurl, url);
        }

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
        public void GivenACorrectRoutesCollection_WhenIAskToCreateAUrlForUserLogout_ThenIGetTheCorrectUrl()
        {
            Assert.AreEqual("/session/delete", GetOutboundUrl(new
                                                                  {
                                                                      controller = "Session",
                                                                      action = "Delete"
                                                                  }));
        }
    }
}