using System.Web;
using System.Web.Routing;
using MBlog;
using MBlog.App_Start;
using Moq;

namespace MBlogUnitTest.Extensions
{
    public static class RouteExtensions
    {
        public static RouteData GetRouteData(this string url, string httpMethod)
        {
            var routes = new RouteCollection();
            routes.RegisterRoutes();
            var mockHttpContext = new Mock<HttpContextBase>();
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(r => r.HttpMethod).Returns(httpMethod);
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockRequest.Setup(x => x.AppRelativeCurrentExecutionFilePath).Returns(url);

            RouteData routeData = routes.GetRouteData(mockHttpContext.Object);

            return routeData;
        }
    }
}