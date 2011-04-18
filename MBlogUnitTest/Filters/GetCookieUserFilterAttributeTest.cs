using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MBlog.Controllers;
using MBlog.Filters;
using MBlog.Infrastructure;
using MBlogModel;
using MBlogRepository;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Filters
{
    [TestFixture]
    class GetCookieUserFilterAttributeTest
    {
        private ActionExecutingContext _actionExecutingContext;
        private IUserRepository _userRepository;
        Mock<HttpContextBase> _mockHttpContext;

        [SetUp]
        public void SetUp()
        {
            var mockRepo = new Mock<IUserRepository>();
            var user = new User();
            user.AddUserDetails("Name", "EMail", "Password", false);
            mockRepo.Setup(r => r.GetUser(1)).Returns(user);

            _userRepository = mockRepo.Object;
            _mockHttpContext = new Mock<HttpContextBase>();
            _mockHttpContext.SetupProperty(h => h.User);

            HttpContextBase httpContextBase = _mockHttpContext.Object;


            ControllerContext controllerContext =
            new ControllerContext(httpContextBase,
                                  new RouteData(),
                                  new BaseController(_userRepository));

            var actionDescriptor = new Mock<ActionDescriptor>();
            actionDescriptor.SetupGet(x => x.ActionName).Returns("Action_With_SomeAttribute");

            _actionExecutingContext =
                new ActionExecutingContext(controllerContext,
                                           actionDescriptor.Object,
                                           new RouteValueDictionary());

        }

        [Test]
        public void GivenANonBaseController_WhenIExecuteTheFilter_ThenTheFilterDoesNothing()
        {
            ControllerContext controllerContext =
            new ControllerContext(new Mock<HttpContextBase>().Object,
                                  new RouteData(),
                                  new FakeController());

            var actionDescriptor = new Mock<ActionDescriptor>();
            actionDescriptor.SetupGet(x => x.ActionName).Returns("Action_With_SomeAttribute");

            _actionExecutingContext =
                new ActionExecutingContext(controllerContext,
                                           actionDescriptor.Object,
                                           new RouteValueDictionary());

            GetCookieUserFilterAttribute attribute = new GetCookieUserFilterAttribute();

            attribute.OnActionExecuting(_actionExecutingContext);
        }

        [Test]
        public void GivenAFilter_WhenThereIsACookieInTheContext_AndTheUserIdIsValid_ThenALoggedUserIsReturnedInTheHttpContext()
        {
            _mockHttpContext.Setup(h => h.Request).Returns(new FakeRequestWithValidUserId());
            GetCookieUserFilterAttribute attribute = new GetCookieUserFilterAttribute();

            attribute.OnActionExecuting(_actionExecutingContext);

            Assert.That(_mockHttpContext.Object.User, Is.Not.Null);
            Assert.That(_mockHttpContext.Object.User.Identity.IsAuthenticated, Is.True);
        }

        [Test]
        public void GivenAFilter_WhenThereIsACookieInTheContext_AndTheUserIdIsInValid_ThenNoLoggedInUserIsReturnedInTheHttpContext()
        {
            _mockHttpContext.Setup(h => h.Request).Returns(new FakeRequestWithInvalidUserId());
            GetCookieUserFilterAttribute attribute = new GetCookieUserFilterAttribute();

            attribute.OnActionExecuting(_actionExecutingContext);

            Assert.That(_mockHttpContext.Object.User, Is.Not.Null);
            Assert.That(_mockHttpContext.Object.User.Identity.IsAuthenticated, Is.False);                
        }
    }

    class FakeController : Controller
    { }

    class FakeRequestWithValidUserId : HttpRequestBase
    {
        public override HttpCookieCollection Cookies
        {
            get
            {
                HttpCookieCollection collection = new HttpCookieCollection();
                byte[] cipherText = "1".Encrypt();
                string cookie = Convert.ToBase64String(cipherText);
                collection.Add(new HttpCookie("USER", cookie));
                return collection;
            }
        }
    }

    class FakeRequestWithInvalidUserId : HttpRequestBase
    {
        public override HttpCookieCollection Cookies
        {
            get
            {
                HttpCookieCollection collection = new HttpCookieCollection();
                byte[] cipherText = "2".Encrypt();
                string cookie = Convert.ToBase64String(cipherText);
                collection.Add(new HttpCookie("USER", cookie));
                return collection;
            }
        }
    }
}
