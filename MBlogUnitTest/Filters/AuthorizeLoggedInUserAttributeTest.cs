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
        private Mock<HttpContextBase> _mockHttpContext;
        Mock<RequestContext> _requestContext;

        [SetUp]
        public void SetUp()
        {
            _mockHttpContext = new Mock<HttpContextBase>();
            _mockHttpContext = new Mock<HttpContextBase>();
            _requestContext = new Mock<RequestContext>();
            _mockHttpContext.Setup(h => h.CurrentHandler).Returns(new MvcHandler(_requestContext.Object));
            _mockHttpContext.Setup(h => h.Items).Returns(new Dictionary<string, object>());
            _mockHttpContext.Setup(h => h.Response).Returns(new FakeResponse());
        }

        [Test]
        public void GivenAFilter_WhenTheUserIsNotLoggedIn_ThenTheFilterReturnsFalse()
        {
            var filterContext = CreateFilterContext();
            var model = new UserViewModel { IsLoggedIn = false };
            _mockHttpContext.Setup(h => h.User).Returns(model);
            var loggedInUserAttribute = new AuthorizeLoggedInUserAttribute();
            loggedInUserAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        public void GivenAFilter_WhenTheUserIsNull_ThenTheFilterReturnsFalse()
        {
            var filterContext = CreateFilterContext();
            _mockHttpContext.Setup(h => h.User).Returns((IPrincipal)null);
            var loggedInUserAttribute = new AuthorizeLoggedInUserAttribute();
            loggedInUserAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        public void GivenAFilter_WhenTheUserIsLoggedIn_ThenTheFilterReturnsTrue()
        {
            var filterContext = CreateFilterContext();
            var model = new UserViewModel { IsLoggedIn = true };
            _mockHttpContext.Setup(h => h.User).Returns(model);
            var loggedInUserAttribute = new AuthorizeLoggedInUserAttribute();
            loggedInUserAttribute.OnAuthorization(filterContext);
            Assert.That(filterContext.Result, Is.Null);
        }

        private AuthorizationContext CreateFilterContext()
        {
            RouteData routeData = "~/kevin/edit/25/1".GetRouteData("GET");
            var actionDescriptor = new Mock<ActionDescriptor>();
            actionDescriptor.SetupGet(x => x.ActionName).Returns("Action_With_SomeAttribute");
            actionDescriptor.SetupGet(x => x.ControllerDescriptor).Returns(
                new ReflectedControllerDescriptor(typeof(BaseController)));
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