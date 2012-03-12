using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IoC;
using MBlog.Controllers;
using MBlog.Filters;
using MBlog.Models.User;
using MBlogDomainInterfaces;
using MBlogModel;
using MBlogRepository.Interfaces;
using MBlogUnitTest.Extensions;
using MBlogUnitTest.Helpers;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Filters
{
    [TestFixture]
    public class AuthorizeBlogOwnerAttributeTest
    {
        private IBlogDomain _blogDomain;
        private Mock<HttpContextBase> _mockHttpContext;
        private const string Nickname = "nickname";
        Mock<RequestContext> _requestContext;
        Mock<IBlogDomain> _mockBlogDomain;
        private AuthorizeBlogOwnerAttribute _blogOwnerAttribute;

        [SetUp]
        public void SetUp()
        {
            _mockBlogDomain = new Mock<IBlogDomain>();

            _mockHttpContext = new Mock<HttpContextBase>();
            _requestContext = new Mock<RequestContext>();
            _mockHttpContext.SetupProperty(h => h.User);
            _mockHttpContext.Setup(h => h.CurrentHandler).Returns(new MvcHandler(_requestContext.Object));

            _mockHttpContext.Setup(h => h.Items).Returns(new Dictionary<string, object>());

            _mockHttpContext.Setup(h => h.Response).Returns(new FakeResponse());

            _blogDomain = _mockBlogDomain.Object;
            _blogOwnerAttribute = new AuthorizeBlogOwnerAttribute();
            _blogOwnerAttribute.BlogDomain = _blogDomain;
        }

        [Test]
        public void GivenAFilter_WhenTheBlogIdIsEmpty_ThenTheFilterReturnsFalse()
        {
            var routeData = string.Format("~/{0}/edit/25", Nickname).GetRouteData("GET");
            _requestContext.Setup(r => r.RouteData).Returns(routeData);

            var filterContext = CreateFilterContext(routeData);
            var httpRequest = new Mock<HttpRequestBase>();
            _mockHttpContext.Setup(h => h.Request).Returns(httpRequest.Object);
            

            _blogOwnerAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        public void GivenAFilter_WhenTheUserOwnsThisBlog_ThenTheFilterReturnsTrue()
        {
            const int blogId = 1;
            var routeData = string.Format("~/{0}/edit/{1}/25", Nickname, blogId).GetRouteData("GET");
            _mockBlogDomain.Setup(r => r.GetBlog(Nickname)).Returns(new Blog{Id = blogId});
            _requestContext.Setup(r => r.RouteData).Returns(routeData);

            var filterContext = CreateFilterContext(routeData);

            var model = new UserViewModel{IsLoggedIn = true};
            _mockHttpContext.Setup(h => h.User).Returns(model);

            _blogOwnerAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.Null);
        }

        [Test]
        public void GivenAFilter_WhenTheUserIsNotLogedIn_ThenTheFilterReturnsFalse()
        {
            const int blogId = 1;
            var routeData = string.Format("~/{0}/edit/{1}/25", Nickname, blogId).GetRouteData("GET");
            _mockBlogDomain.Setup(r => r.GetBlog(Nickname)).Returns(new Blog { Id = blogId });
            _requestContext.Setup(r => r.RouteData).Returns(routeData);

            var filterContext = CreateFilterContext(routeData);

            var model = new UserViewModel { IsLoggedIn = false };
            _mockHttpContext.Setup(h => h.User).Returns(model);

            _blogOwnerAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        public void GivenAFilter_WhenTheUserDoesNotOwnTheBlog_ThenTheFilterReturnsFalse()
        {
            const int blogId = 1;
            var routeData = string.Format("~/{0}/edit/{1}/25", "wrong-nickname", blogId).GetRouteData("GET");
            _mockBlogDomain.Setup(r => r.GetBlog(Nickname)).Returns(new Blog { Id = blogId });
            _requestContext.Setup(r => r.RouteData).Returns(routeData);

            var filterContext = CreateFilterContext(routeData);

            var model = new UserViewModel { IsLoggedIn = true };
            _mockHttpContext.Setup(h => h.User).Returns(model);

            _blogOwnerAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        public void GivenAFilter_BlogIdIsInRequestItems_ThenTheFilterReturnsTrue()
        {
            int blogId = 1;
            var routeData = string.Format("~/{0}/update/25", Nickname).GetRouteData("POST");
            _mockBlogDomain.Setup(r => r.GetBlog(Nickname)).Returns(new Blog { Id = blogId });
            _requestContext.Setup(r => r.RouteData).Returns(routeData);

            var filterContext = CreateFilterContext(routeData);
            
            var httpRequest = new Mock<HttpRequestBase>();
            var qs = new NameValueCollection();
            qs.Add("blogId",blogId.ToString());
            httpRequest.Setup(h => h.Params).Returns(qs);
            httpRequest.Setup(h => h.QueryString).Returns(qs);

            var model = new UserViewModel { IsLoggedIn = true };
            _mockHttpContext.Setup(h => h.User).Returns(model);
            _mockHttpContext.Setup(h => h.Request).Returns(new FakeRequest());
            _blogOwnerAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.Null);
        }


        private AuthorizationContext CreateFilterContext(RouteData routeData)
        {
            var actionDescriptor = new Mock<ActionDescriptor>();
            actionDescriptor.SetupGet(x => x.ActionName).Returns("Action_With_SomeAttribute");
            actionDescriptor.SetupGet(x => x.ControllerDescriptor).Returns(
                new ReflectedControllerDescriptor(typeof (BaseController)));
            var controllerContext = CreateControllerContext(routeData);
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
    }
}