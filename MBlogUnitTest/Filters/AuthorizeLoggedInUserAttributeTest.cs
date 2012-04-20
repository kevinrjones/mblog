using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MBlog.Controllers;
using MBlog.Filters;
using MBlog.Models.User;
using MBlogUnitTest.Extensions;
using MBlogUnitTest.Helpers;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Filters
{
    [TestFixture]
    public class AuthorizeLoggedInUserAttributeTest
    {

        [SetUp]
        public void SetUp()
        {
            _mockHttpContext = new Mock<HttpContextBase>();
            _requestContext = new Mock<RequestContext>();
            _mockHttpContext.Setup(h => h.CurrentHandler).Returns(new MvcHandler(_requestContext.Object));
            _mockHttpContext.Setup(h => h.Items).Returns(new Dictionary<string, object>());
            _mockHttpContext.Setup(h => h.Response).Returns(new FakeResponse());
        }

        private Mock<HttpContextBase> _mockHttpContext;
        private Mock<RequestContext> _requestContext;

        private AuthorizationContext CreateFilterContext()
        {
            RouteData routeData = "~/kevin/edit/25/1".GetRouteData("GET");
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
        public void GivenAFilter_WhenTheUserIsLoggedIn_ThenTheFilterReturnsTrue()
        {
            AuthorizationContext filterContext = CreateFilterContext();
            var model = new UserViewModel {IsLoggedIn = true};
            _mockHttpContext.Setup(h => h.User).Returns(model);
            var loggedInUserAttribute = new AuthorizeLoggedInUserAttribute();
            loggedInUserAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.Null);
        }

        [Test]
        public void GivenAFilter_WhenTheUserIsNotLoggedIn_ThenTheFilterReturnsFalse()
        {
            AuthorizationContext filterContext = CreateFilterContext();
            var model = new UserViewModel {IsLoggedIn = false};
            _mockHttpContext.Setup(h => h.User).Returns(model);
            var loggedInUserAttribute = new AuthorizeLoggedInUserAttribute();
            loggedInUserAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        public void GivenAFilter_WhenTheUserIsNull_ThenTheFilterReturnsFalse()
        {
            AuthorizationContext filterContext = CreateFilterContext();
            _mockHttpContext.Setup(h => h.User).Returns((IPrincipal) null);
            var loggedInUserAttribute = new AuthorizeLoggedInUserAttribute();
            loggedInUserAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.TypeOf<RedirectResult>());
        }
    }
}