using System;
using System.Web;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Filters;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    class UserControllerTest : BaseControllerTests
    {
        UserController _controller;
        private Mock<IUserRepository> _userRepository;
        private Mock<IUsernameBlacklistRepository> _usernameBlacklistRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _usernameBlacklistRepository = new Mock<IUsernameBlacklistRepository>();

            _controller = new UserController(_userRepository.Object, _usernameBlacklistRepository.Object, null);

            SetControllerContext(_controller);
        }

        [Test]
        public void GivenAUser_WhenIRegister_AndTheUserIsAlreadyAuthenticated_ThenIGetRedirectedToTheAdminPage()
        {
            UserViewModel userViewModel = new UserViewModel { IsLoggedIn = true};

            MockHttpContext.Setup(h => h.User).Returns(userViewModel);
            RedirectToRouteResult result = _controller.New() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Dashboard").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("index").IgnoreCase);
        }

        [Test]
        public void GivenAUser_WhenIRegister_AndTheUserIsNotLoggdIn_ThenTheUserIsSentToTheRegistrationPage()
        {
            UserViewModel userViewModel = new UserViewModel { IsLoggedIn = false };

            MockHttpContext.Setup(h => h.User).Returns(userViewModel);
            ViewResult result = _controller.New() as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAUser_WhenIRegister_AndTheUserIsAlreadyRegistered_ThenIGetRedirectedToTheRegistrationPage()
        {
            string email = "foo@bar.com";
            UserViewModel userViewModel = new UserViewModel { Email = email};
            User user = new User("", email, "", false);
            _userRepository.Setup(u => u.GetUser(email)).Returns(user);

            ViewResult result = _controller.Create(userViewModel) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Register").IgnoreCase);
            Assert.That(_controller.ModelState["EMail"].Errors.Count, Is.EqualTo(1));
            Assert.That(_controller.ModelState["EMail"].Errors[0].ErrorMessage, Is.EqualTo("EMail already exists in database"));
        }

        [Test]
        public void GivenAUser_WhenIRegister_AndTheUserIsBlacklisted_ThenIGetRedirectedToTheRegistrationPage()
        {
            string name = "name";
            UserViewModel userViewModel = new UserViewModel { Name = name };

            _usernameBlacklistRepository.Setup(u => u.GetName(name)).Returns(new Blacklist());

            ViewResult result = _controller.Create(userViewModel) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Register").IgnoreCase);
            Assert.That(_controller.ModelState["Name"].Errors.Count, Is.EqualTo(1));
            Assert.That(_controller.ModelState["Name"].Errors[0].ErrorMessage, Is.EqualTo("That user name is reserved"));
        }

        [Test]
        public void GivenAnInvalidUser_WhenIRegister_ThenIGetRedirectedToTheRegistrationPage()
        {
            _controller.ModelState.AddModelError("EMail", "Email error");

            ViewResult result = _controller.Create(new UserViewModel()) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Register").IgnoreCase);
        }

        [Test]
        public void GivenAValidUser_WhenIRegister_AndTheRegistrationIsSuccesful_ThenIGetRedirectedToTheAdminPage()
        {
            RedirectToRouteResult result = _controller.Create(new UserViewModel()) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Dashboard").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index").IgnoreCase);
        }
        
        [Test]
        public void GivenALoggdInUser_WhenILogout_ThenTheCookieIsExpired()
        {
            var cookies = new HttpCookieCollection();
            string cookieName = "USER";

            cookies.Add(new HttpCookie(cookieName));
            MockRequest.Setup(r => r.Cookies).Returns(cookies);

            _controller.Logout();
            HttpCookie cookie = FakeResponse.Cookies[GetCookieUserFilterAttribute.UserCookie];
            Assert.That(cookie.Expires, Is.EqualTo(new DateTime(1970, 1, 1)));
        }

    }
}

