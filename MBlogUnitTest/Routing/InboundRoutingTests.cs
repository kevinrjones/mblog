using System.Web;
using MBlog;
using NUnit.Framework;
using System.Web.Routing;

namespace MBlogUnitTest.Routing
{
    [TestFixture]
    public class InboundRoutingTests
    {
        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForSlashPost_ThenIGetTheIndexView()
        {
            TestRoute("~/Post", new
                                     {
                                         controller = "Post",
                                         action = "Index"
                                     });
        }


        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForTheErrorPage_ThenIGetTheErrorInde()
        {
            TestRoute("~/Error", new
            {
                controller = "Error",
                action = "Index"
            });
        }

        private void TestRoute(string url, object expectedValues)
        {
            // Arrange: Prepare the route collection and a mock request context
            RouteCollection routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            var mockHttpContext = new Moq.Mock<HttpContextBase>();
            var mockRequest = new Moq.Mock<HttpRequestBase>();
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockRequest.Setup(x => x.AppRelativeCurrentExecutionFilePath).Returns(url);

            // Act: Get the mapped route
            RouteData routeData = routes.GetRouteData(mockHttpContext.Object);

            // Assert: Test the route values against expectations
            Assert.That(routeData, Is.Not.Null);
            var routeValueDictionaryExpected = new RouteValueDictionary(expectedValues);
            foreach (var expectedRouteValue in routeValueDictionaryExpected)
            {
                if (expectedRouteValue.Value == null)
                {
                    Assert.That(routeData.Values[expectedRouteValue.Key], Is.Null);
                }
                else
                {
                    Assert.That(expectedRouteValue.Value.ToString(), Is.EqualTo(
                        routeData.Values[expectedRouteValue.Key].ToString()).IgnoreCase);
                }
            }
        }

    }
}