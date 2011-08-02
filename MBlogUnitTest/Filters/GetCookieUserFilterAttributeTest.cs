using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MBlog.Controllers;
using MBlog.Filters;
using MBlog.Infrastructure;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Filters
{
    [TestFixture]
    class GetCookieUserFilterAttributeTest
    {
        private AuthorizationContext _actionExecutingContext;
        private IUserRepository _userRepository;
        Mock<HttpContextBase> _mockHttpContext;
        private const string Nickname = "nickname";

        [SetUp]
        public void SetUp()
        {           
            List<Blog> blogs = new List<Blog> { new Blog { Nickname = Nickname } };

            var user = new User("Name", "EMail", "Password", false);
            user.Blogs = blogs;

            var mockRepo = new Mock<IUserRepository>();

            mockRepo.Setup(r => r.GetUser(1)).Returns(user);
            mockRepo.Setup(r => r.GetUserWithTheirBlogs(1)).Returns(user);

            var user2 = new User("Name", "EMail", "Password", false);

            mockRepo.Setup(r => r.GetUser(2)).Returns(user2);
            mockRepo.Setup(r => r.GetUserWithTheirBlogs(2)).Returns(user2);

            _userRepository = mockRepo.Object;
            _mockHttpContext = new Mock<HttpContextBase>();
            _mockHttpContext.SetupProperty(h => h.User);

            HttpContextBase httpContextBase = _mockHttpContext.Object;


            ControllerContext controllerContext =
            new ControllerContext(httpContextBase,
                                  new RouteData(),
                                  new BaseController(_userRepository, null));

            var actionDescriptor = new Mock<ActionDescriptor>();
            actionDescriptor.SetupGet(x => x.ActionName).Returns("Action_With_SomeAttribute");

            _actionExecutingContext =
                new AuthorizationContext(controllerContext,
                                           actionDescriptor.Object);

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
                new AuthorizationContext(controllerContext,
                                           actionDescriptor.Object);

            GetCookieUserFilterAttribute attribute = new GetCookieUserFilterAttribute();

            attribute.OnAuthorization(_actionExecutingContext);
        }

        [Test]
        public void GivenAFilter_WhenThereIsACookieInTheContext_AndTheUserIdIsValid_ThenALoggedUserIsReturnedInTheHttpContext()
        {
            _mockHttpContext.Setup(h => h.Request).Returns(new FakeRequestWithValidUserId());
            GetCookieUserFilterAttribute attribute = new GetCookieUserFilterAttribute();

            attribute.OnAuthorization(_actionExecutingContext);

            Assert.That(_mockHttpContext.Object.User, Is.Not.Null);
            Assert.That(_mockHttpContext.Object.User.Identity.IsAuthenticated, Is.True);
        }

        [Test]
        public void GivenAFilter_WhenThereIsACookieInTheContext_AndTheUserIdIsInValid_ThenNoLoggedInUserIsReturnedInTheHttpContext()
        {
            _mockHttpContext.Setup(h => h.Request).Returns(new FakeRequestWithInvalidUserId());
            GetCookieUserFilterAttribute attribute = new GetCookieUserFilterAttribute();

            attribute.OnAuthorization(_actionExecutingContext);

            Assert.That(_mockHttpContext.Object.User, Is.Not.Null);
            Assert.That(_mockHttpContext.Object.User.Identity.IsAuthenticated, Is.False);                
        }

        [Test]
        public void GivenAUserWithThatOwnsBlogs_WhenTheFilterExecutes_NicknamesAreAddedToTheUser()
        {
            _mockHttpContext.Setup(h => h.Request).Returns(new FakeRequestWithValidUserId());
            GetCookieUserFilterAttribute attribute = new GetCookieUserFilterAttribute();

            attribute.OnAuthorization(_actionExecutingContext);
            UserViewModel userViewModel = (UserViewModel) _mockHttpContext.Object.User;
            
            Assert.That(userViewModel.Nicknames.Count, Is.GreaterThan(0));
            Assert.That(userViewModel.IsBlogOwner(Nickname), Is.True);
        }

        [Test]
        public void GetNoNicknamesWhenNotLoggedIn()
        {
            _mockHttpContext.Setup(h => h.Request).Returns(new FakeRequestWithInvalidUserId());
            GetCookieUserFilterAttribute attribute = new GetCookieUserFilterAttribute();

            attribute.OnAuthorization(_actionExecutingContext);
            UserViewModel userViewModel = (UserViewModel)_mockHttpContext.Object.User;

            Assert.That(userViewModel.Nicknames.Count, Is.EqualTo(0));
            Assert.That(userViewModel.IsBlogOwner(Nickname), Is.False);
        }

        [Test]
        public void GivenAUserWithThatOwnsNoBlogs_WhenTHeFilterExecutes_NoNicknamesAreAddedToTheUser()
        {
            _mockHttpContext.Setup(h => h.Request).Returns(new FakeRequestWithValidUserIdButNoBlogs());
            GetCookieUserFilterAttribute attribute = new GetCookieUserFilterAttribute();

            attribute.OnAuthorization(_actionExecutingContext);
            UserViewModel userViewModel = (UserViewModel)_mockHttpContext.Object.User;

            Assert.That(userViewModel.Nicknames.Count, Is.EqualTo(0));
            Assert.That(userViewModel.IsBlogOwner(Nickname), Is.False);
        }
    }

    class FakeController : Controller
    { }

    class FakeRequestWithValidUserIdButNoBlogs : HttpRequestBase
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
                byte[] cipherText = "3".Encrypt();
                string cookie = Convert.ToBase64String(cipherText);
                collection.Add(new HttpCookie("USER", cookie));
                return collection;
            }
        }
    }
}
