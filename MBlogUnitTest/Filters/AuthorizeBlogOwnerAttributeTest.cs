using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MBlog.Controllers;
using MBlog.Filters;
using MBlog.Models.User;
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
        private IBlogRepository _blogRepository;
        private Mock<HttpContextBase> _mockHttpContext;
        private const string Nickname = "nickname";
        Mock<RequestContext> _requestContext;
        Mock<IBlogRepository> _mockBlogRepository;

        [SetUp]
        public void SetUp()
        {
            _mockBlogRepository = new Mock<IBlogRepository>();

            _mockHttpContext = new Mock<HttpContextBase>();
            _requestContext = new Mock<RequestContext>();
            _mockHttpContext.SetupProperty(h => h.User);
            _mockHttpContext.Setup(h => h.CurrentHandler).Returns(new MvcHandler(_requestContext.Object));

            _mockHttpContext.Setup(h => h.Items).Returns(new Dictionary<string, object>());

            _mockHttpContext.Setup(h => h.Response).Returns(new FakeResponse());

            _blogRepository = _mockBlogRepository.Object;
        }

        [Test]
        public void GivenAFilter_BlogIdIsEmpty_ThenTheFilterReturnsFalse()
        {
            var routeData = string.Format("~/{0}/edit/25", Nickname).GetRouteData("GET");
            _requestContext.Setup(r => r.RouteData).Returns(routeData);

            var filterContext = CreateFilterContext(routeData);

            var blogOwnerAttribute = new AuthorizeBlogOwnerAttribute();
            blogOwnerAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        public void GivenAFilter_WhenTheUserOwnsThisBlog_ThenTheFilterReturnsTrue()
        {
            const int blogId = 1;
            var routeData = string.Format("~/{0}/edit/{1}/25", Nickname, blogId).GetRouteData("GET");
            _mockBlogRepository.Setup(r => r.GetBlog(Nickname)).Returns(new Blog{Id = blogId});
            _requestContext.Setup(r => r.RouteData).Returns(routeData);

            var filterContext = CreateFilterContext(routeData);

            var model = new UserViewModel{IsLoggedIn = true};
            _mockHttpContext.Setup(h => h.User).Returns(model);

            var blogOwnerAttribute = new AuthorizeBlogOwnerAttribute();
            blogOwnerAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.Null);
        }

        [Test]
        public void GivenAFilter_WhenTheUserIsNotLogedIn_ThenTheFilterReturnsFalse()
        {
            const int blogId = 1;
            var routeData = string.Format("~/{0}/edit/{1}/25", Nickname, blogId).GetRouteData("GET");
            _mockBlogRepository.Setup(r => r.GetBlog(Nickname)).Returns(new Blog { Id = blogId });
            _requestContext.Setup(r => r.RouteData).Returns(routeData);

            var filterContext = CreateFilterContext(routeData);

            var model = new UserViewModel { IsLoggedIn = false };
            _mockHttpContext.Setup(h => h.User).Returns(model);

            var blogOwnerAttribute = new AuthorizeBlogOwnerAttribute();
            blogOwnerAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        public void GivenAFilter_WhenTheUserDoesNotOwnTheBlog_ThenTheFilterReturnsFalse()
        {
            const int blogId = 1;
            var routeData = string.Format("~/{0}/edit/{1}/25", "wrong-nickname", blogId).GetRouteData("GET");
            _mockBlogRepository.Setup(r => r.GetBlog(Nickname)).Returns(new Blog { Id = blogId });
            _requestContext.Setup(r => r.RouteData).Returns(routeData);

            var filterContext = CreateFilterContext(routeData);

            var model = new UserViewModel { IsLoggedIn = true };
            _mockHttpContext.Setup(h => h.User).Returns(model);

            var blogOwnerAttribute = new AuthorizeBlogOwnerAttribute();
            blogOwnerAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.TypeOf<RedirectResult>());
        }

        private AuthorizationContext CreateFilterContext(RouteData routeData)
        {
            var actionDescriptor = new Mock<ActionDescriptor>();
            actionDescriptor.SetupGet(x => x.ActionName).Returns("Action_With_SomeAttribute");
            var controllerContext = CreateControllerContext(routeData);
            return new AuthorizationContext(controllerContext,
                                         actionDescriptor.Object);
        }

        private ControllerContext CreateControllerContext(RouteData routeData)
        {
            var controllerContext =
                new ControllerContext(_mockHttpContext.Object,
                                      routeData,
                                      new BaseController(null, _blogRepository));
            return controllerContext;
        }
    }
}