using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Filters;
using MBlog.Models.User;
using MBlogModel;
using MBlogServiceInterfaces;
using MBlogServiceInterfaces.ModelState;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    internal class UserControllerTest : BaseControllerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _userDomain = new Mock<IUserService>();
            _controller = new UserController(_userDomain.Object, null);

            SetControllerContext(_controller);
        }

        #endregion

        private UserController _controller;
        private Mock<IUserService> _userDomain;

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

        [Test]
        public void GivenAUser_WhenIRegister_AndTheUserIsAlreadyAuthenticated_ThenIGetRedirectedToTheAdminPage()
        {
            var userViewModel = new UserViewModel {IsLoggedIn = true};

            MockHttpContext.Setup(h => h.User).Returns(userViewModel);
            var result = _controller.New() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Dashboard").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("index").IgnoreCase);
        }

        [Test]
        public void GivenAUser_WhenIRegister_AndTheUserIsAlreadyRegistered_ThenIGetRedirectedToTheRegistrationPage()
        {
            string email = "foo@bar.com";
            var userViewModel = new UserViewModel {Email = email};
            _userDomain.Setup(u => u.IsUserRegistrationValid(It.IsAny<string>(), email)).Returns(
                new List<ErrorDetails>
                    {new ErrorDetails {FieldName = "EMail", Message = "EMail already exists in database"}});

            var result = _controller.Create(userViewModel) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Register").IgnoreCase);
            Assert.That(_controller.ModelState["EMail"].Errors.Count, Is.EqualTo(1));
            Assert.That(_controller.ModelState["EMail"].Errors[0].ErrorMessage,
                        Is.EqualTo("EMail already exists in database"));
        }

        [Test]
        public void GivenAUser_WhenIRegister_AndTheUserIsBlacklisted_ThenIGetRedirectedToTheRegistrationPage()
        {
            string name = "name";
            var userViewModel = new UserViewModel {Name = name};

            _userDomain.Setup(u => u.IsUserRegistrationValid(name, It.IsAny<string>())).Returns(new List<ErrorDetails>
                                                                                                    {
                                                                                                        new ErrorDetails
                                                                                                            {
                                                                                                                FieldName
                                                                                                                    =
                                                                                                                    "Name",
                                                                                                                Message
                                                                                                                    =
                                                                                                                    "That user name is reserved"
                                                                                                            }
                                                                                                    });

            var result = _controller.Create(userViewModel) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Register").IgnoreCase);
            Assert.That(_controller.ModelState["Name"].Errors.Count, Is.EqualTo(1));
            Assert.That(_controller.ModelState["Name"].Errors[0].ErrorMessage, Is.EqualTo("That user name is reserved"));
        }

        [Test]
        public void GivenAUser_WhenIRegister_AndTheUserIsNotLoggdIn_ThenTheUserIsSentToTheRegistrationPage()
        {
            var userViewModel = new UserViewModel {IsLoggedIn = false};

            MockHttpContext.Setup(h => h.User).Returns(userViewModel);
            var result = _controller.New() as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAValidUser_WhenIRegister_AndTheRegistrationIsSuccesful_ThenIGetRedirectedToTheAdminPage()
        {
            _userDomain.Setup(u => u.IsUserRegistrationValid(It.IsAny<string>(), It.IsAny<string>())).Returns(
                new List<ErrorDetails>());
            _userDomain.Setup(u => u.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
                new User());
            var result = _controller.Create(new UserViewModel()) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Dashboard").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index").IgnoreCase);
        }

        [Test]
        public void GivenAnInvalidUser_WhenIRegister_ThenIGetRedirectedToTheRegistrationPage()
        {
            _controller.ModelState.AddModelError("EMail", "Email error");
            var result = _controller.Create(new UserViewModel()) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Register").IgnoreCase);
        }
    }
}