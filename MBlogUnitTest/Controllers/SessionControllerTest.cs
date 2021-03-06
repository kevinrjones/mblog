﻿using System;
using System.Web;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Filters;
using MBlog.Infrastructure;
using MBlog.Models.User;
using MBlogModel;
using MBlogServiceInterfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    internal class SessionControllerTest : BaseControllerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _userDomain = new Mock<IUserService>();

            _sessionController = new SessionController(_userDomain.Object, null);

            SetControllerContext(_sessionController);
        }

        #endregion

        private SessionController _sessionController;
        private Mock<IUserService> _userDomain;

        [Test]
        public void GivenALoggdInUser_WhenILogout_ThenTheCookieIsExpired()
        {
            var cookies = new HttpCookieCollection();
            string cookieName = "USER";

            cookies.Add(new HttpCookie(cookieName));
            MockRequest.Setup(r => r.Cookies).Returns(cookies);

            _sessionController.Delete();
            HttpCookie cookie = FakeResponse.Cookies[GetCookieUserFilterAttribute.UserCookieName];
            Assert.That(cookie.Expires, Is.EqualTo(new DateTime(1970, 1, 1)));
        }

        [Test]
        public void GivenALoggdInUser_WhenILogout_ThenTheUserIsRemovedFromTheContext()
        {
            var cookies = new HttpCookieCollection();
            string cookieName = "USER";

            // Add response cookies
            FakeResponse.Cookies.Add(new HttpCookie(cookieName));
            Assert.That(FakeResponse.Cookies.Count, Is.EqualTo(1));

            // Add request cookies
            cookies.Add(new HttpCookie(cookieName));
            MockRequest.Setup(r => r.Cookies).Returns(cookies);

            MockHttpContext.SetupProperty(h => h.User);
            _sessionController.HttpContext.User = new UserViewModel();

            Assert.That(_sessionController.HttpContext.User, Is.Not.Null);
            var result = _sessionController.Delete() as RedirectToRouteResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(_sessionController.HttpContext.User, Is.Null);
        }

        [Test]
        public void GivenAMatchingPassword_WhenILogin_ThenIGetRedirectedToTheAdminPage()
        {
            string password = "password";
            _userDomain.Setup(u => u.GetUser("email@mail.com")).Returns(new User {Password = password});
            var result =
                _sessionController.Create(new LoginUserViewModel {Email = "email@mail.com", Password = password}) as
                RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Dashboard").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public void GivenANonMatchingPassword_WhenILogin_ThenIGetRedirectedToTheRegistrationPage()
        {
            _userDomain.Setup(u => u.GetUser("email@mail.com")).Returns(new User {Password = "foo"});
            var result =
                _sessionController.Create(new LoginUserViewModel {Email = "email@mail.com", Password = "password"}) as
                ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("New").IgnoreCase);
        }

        [Test]
        public void GivenANotLoggdInUser_WhenILogout_TheUserIsRedirectedToTheHomePage()
        {
            var cookies = new HttpCookieCollection();
            MockRequest.Setup(r => r.Cookies).Returns(cookies);
            var result = _sessionController.Delete() as RedirectToRouteResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Home").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index").IgnoreCase);
        }

        [Test]
        public void GivenAUserThatDoesNotExists_WhenTheyLogin_ThenIGetRedirectedToThePostsPage()
        {
            var result = _sessionController.Create(new LoginUserViewModel()) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("New").IgnoreCase);
        }

        [Test]
        public void GivenAUserThatExists_WhenTheyLogin_ThenIGetRedirectedToTheAdminPage()
        {
            string email = "email";
            var user = new User("", email, "", false);
            _userDomain.Setup(u => u.GetUser(email)).Returns(user);

            var result = _sessionController.Create(new LoginUserViewModel {Email = email}) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["Controller"], Is.EqualTo("Dashboard").IgnoreCase);
            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index").IgnoreCase);
        }

        [Test]
        public void GivenAUserThatExists_WhenTheyLogin_ThenTheCorrectCookieIsSet()
        {
            string email = "email";
            var user = new User("", email, "", false) {Id = 1};

            _userDomain.Setup(u => u.GetUser(email)).Returns(user);

            MockHttpContext.SetupProperty(h => h.User);

            Assert.That(_sessionController.HttpContext.User, Is.Null);
            _sessionController.Create(new LoginUserViewModel {Email = email});

            byte[] cipherText = user.Id.ToString().Encrypt();
            string base64CipherText = Convert.ToBase64String(cipherText);


            Assert.That(FakeResponse.Cookies.Count, Is.EqualTo(1));
            HttpCookie cookie = FakeResponse.Cookies[0];
            Assert.That(cookie.Value, Is.EqualTo(base64CipherText));
        }

        [Test]
        public void GivenAUserThatExists_WhenTheyLogin_ThenTheUserIsInTheContext()
        {
            string email = "email";
            var user = new User("", email, "", false);
            _userDomain.Setup(u => u.GetUser(email)).Returns(user);

            MockHttpContext.SetupProperty(h => h.User);

            Assert.That(_sessionController.HttpContext.User, Is.Null);
            _sessionController.Create(new LoginUserViewModel {Email = email});

            Assert.That(_sessionController.HttpContext.User, Is.Not.Null);
        }

        [Test]
        public void GivenAUserThatExists_WhenTheyLogin_ThenTheUserMarkedAsLoggedIn()
        {
            string email = "email";
            var user = new User("", email, "", false);
            _userDomain.Setup(u => u.GetUser(email)).Returns(user);

            MockHttpContext.SetupProperty(h => h.User);

            Assert.That(_sessionController.HttpContext.User, Is.Null);
            _sessionController.Create(new LoginUserViewModel {Email = email});

            var model = _sessionController.HttpContext.User as UserViewModel;
            Assert.That(model.IsAuthenticated, Is.True);
        }

        [Test]
        public void GivenAnAuthenticatedUser_WhenILogin_ThenIGetTheRedirectView()
        {
            var userViewModel = new UserViewModel {IsLoggedIn = true};

            MockHttpContext.Setup(h => h.User).Returns(userViewModel);
            var result = _sessionController.New() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index").IgnoreCase);
        }

        [Test]
        public void GivenAnInvalidEMail_WhenILogin_ThenIGetRedirectedToTheRegistrationPage()
        {
            _sessionController.ModelState.AddModelError("EMail", "Email error");

            var result = _sessionController.Create(new LoginUserViewModel()) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("New").IgnoreCase);
        }

        [Test]
        public void GivenAnInvalidUser_WhenTheyLogin_ThenIGetReturnedToTheLoginView()
        {
            string email = "email";
            var user = new User("", email, "", false);

            _userDomain.Setup(u => u.GetUser(email)).Returns(user);

            var result = _sessionController.Create(new LoginUserViewModel()) as ViewResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("New"));
        }

        [Test]
        public void GivenNoAuthenticatedUser_WhenILogin_ThenIGetTheLoginView()
        {
            MockHttpContext.Setup(h => h.User).Returns(new UserViewModel());
            ActionResult result = _sessionController.New();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }
    }
}