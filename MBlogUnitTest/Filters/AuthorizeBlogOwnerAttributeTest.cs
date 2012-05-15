using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MBlog.Controllers;
using MBlog.Filters;
using MBlog.Models.User;
using MBlogModel;
using MBlogServiceInterfaces;
using MBlogUnitTest.Extensions;
using MBlogUnitTest.Helpers;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Filters
{
    [TestFixture]
    public class AuthorizeBlogOwnerAttributeTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _mockBlogDomain = new Mock<IBlogService>();

            _mockHttpContext = new Mock<HttpContextBase>();
            _requestContext = new Mock<RequestContext>();
            _mockHttpContext.SetupProperty(h => h.User);
            _mockHttpContext.Setup(h => h.CurrentHandler).Returns(new MvcHandler(_requestContext.Object));

            _mockHttpContext.Setup(h => h.Items).Returns(new Dictionary<string, object>());

            _mockHttpContext.Setup(h => h.Response).Returns(new FakeResponse());

            _blogService = _mockBlogDomain.Object;
            _blogOwnerAttribute = new AuthorizeBlogOwnerAttribute();
            _blogOwnerAttribute.BlogService = _blogService;
        }

        #endregion

        private IBlogService _blogService;
        private Mock<HttpContextBase> _mockHttpContext;
        private const string Nickname = "nickname";
        private Mock<RequestContext> _requestContext;
        private Mock<IBlogService> _mockBlogDomain;
        private AuthorizeBlogOwnerAttribute _blogOwnerAttribute;


        private AuthorizationContext CreateFilterContext(RouteData routeData)
        {
            var actionDescriptor = new Mock<ActionDescriptor>();
            actionDescriptor.SetupGet(x => x.ActionName).Returns("Action_With_SomeAttribute");
            actionDescriptor.SetupGet(x => x.ControllerDescriptor).Returns(
                new ReflectedControllerDescriptor(typeof (BaseController)));
            ControllerContext controllerContext = CreateControllerContext(routeData);
            return new AuthorizationContext(controllerContext,
                                            actionDescriptor.Object);
        }

        private ControllerContext CreateControllerContext(RouteData routeData)
        {
            var controllerContext =
                new ControllerContext(_mockHttpContext.Object,
                                      routeData,
                                      new BaseController(null));
            return controllerContext;
        }

        [Test]
        public void GivenAFilter_BlogIdIsInRequestItems_ThenTheFilterReturnsTrue()
        {
            int blogId = 1;
            RouteData routeData = string.Format("~/{0}/update/", Nickname).GetRouteData("POST");
            _mockBlogDomain.Setup(r => r.GetBlog(Nickname)).Returns(new Blog {Id = blogId});
            _requestContext.Setup(r => r.RouteData).Returns(routeData);

            AuthorizationContext filterContext = CreateFilterContext(routeData);

            var httpRequest = new Mock<HttpRequestBase>();
            var queryString = new NameValueCollection();
            queryString.Add("blogId", blogId.ToString());
            httpRequest.Setup(h => h.Params).Returns(queryString);
            httpRequest.Setup(h => h.QueryString).Returns(queryString);

            var model = new UserViewModel {IsLoggedIn = true};
            _mockHttpContext.Setup(h => h.User).Returns(model);
            _mockHttpContext.Setup(h => h.Request).Returns(new FakeRequest());
            _blogOwnerAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.Null);
        }

        [Test]
        public void GivenAFilter_WhenTheBlogIdIsEmpty_ThenTheFilterReturnsFalse()
        {
            RouteData routeData = string.Format("~/{0}/edit/25/1", Nickname).GetRouteData("GET");
            _requestContext.Setup(r => r.RouteData).Returns(routeData);

            AuthorizationContext filterContext = CreateFilterContext(routeData);
            var httpRequest = new Mock<HttpRequestBase>();
            _mockHttpContext.Setup(h => h.Request).Returns(httpRequest.Object);


            _blogOwnerAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        public void GivenAFilter_WhenTheUserDoesNotOwnTheBlog_ThenTheFilterReturnsFalse()
        {
            const int blogId = 1;
            RouteData routeData = string.Format("~/{0}/edit/{1}/25", "wrong-nickname", blogId).GetRouteData("GET");
            _mockBlogDomain.Setup(r => r.GetBlog(Nickname)).Returns(new Blog {Id = blogId});
            _requestContext.Setup(r => r.RouteData).Returns(routeData);

            AuthorizationContext filterContext = CreateFilterContext(routeData);

            var model = new UserViewModel {IsLoggedIn = true};
            _mockHttpContext.Setup(h => h.User).Returns(model);

            _blogOwnerAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        public void GivenAFilter_WhenTheUserIsNotLogedIn_ThenTheFilterReturnsFalse()
        {
            const int blogId = 1;
            RouteData routeData = string.Format("~/{0}/edit/{1}/25", Nickname, blogId).GetRouteData("GET");
            _mockBlogDomain.Setup(r => r.GetBlog(Nickname)).Returns(new Blog {Id = blogId});
            _requestContext.Setup(r => r.RouteData).Returns(routeData);

            AuthorizationContext filterContext = CreateFilterContext(routeData);

            var model = new UserViewModel {IsLoggedIn = false};
            _mockHttpContext.Setup(h => h.User).Returns(model);

            _blogOwnerAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        public void GivenAFilter_WhenTheUserOwnsThisBlog_ThenTheFilterReturnsTrue()
        {
            const int blogId = 1;
            RouteData routeData = string.Format("~/{0}/edit/{1}/25", Nickname, blogId).GetRouteData("GET");
            _mockBlogDomain.Setup(r => r.GetBlog(Nickname)).Returns(new Blog {Id = blogId});
            _requestContext.Setup(r => r.RouteData).Returns(routeData);

            AuthorizationContext filterContext = CreateFilterContext(routeData);

            var model = new UserViewModel {IsLoggedIn = true};
            _mockHttpContext.Setup(h => h.User).Returns(model);

            _blogOwnerAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.Null);
        }
    }
}