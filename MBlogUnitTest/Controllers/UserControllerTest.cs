using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MBlog.Controllers;
using MBlog.Models;
using MBlogModel;
using MBlogRepository;
using MBlogUnitTest.Helpers;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    class UserControllerTest
    {
        Mock<HttpContextBase> _mockHttpContext;
        Mock<HttpRequestBase> _mockRequest;
        FakeResponse _fakeResponse;
        ControllerContext _controllerContext;
        UserController _controller;
        private Mock<IUserRepository> _userRepository;

        [SetUp]
        public void Setup()
        {
            _mockHttpContext = new Mock<HttpContextBase>();
            _mockRequest = new Mock<HttpRequestBase>();
            _fakeResponse = new FakeResponse();
            _userRepository = new Mock<IUserRepository>();

            _mockHttpContext.Setup(m => m.Request).Returns(_mockRequest.Object);
            _mockHttpContext.Setup(m => m.Response).Returns(_fakeResponse);

            _controller = new UserController(_userRepository.Object);
            _controllerContext = new ControllerContext(_mockHttpContext.Object, new RouteData(), _controller);

            _controller.ControllerContext = _controllerContext;

        }

        [Test]
        public void GivenNoAuthenticatedUser_WhenILogin_ThenIGetTheLoginView()
        {
            _mockHttpContext.Setup(h => h.User).Returns(new UserViewModel());
            ActionResult result = _controller.Login();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void GivenAnAuthenticatedUser_WhenILogin_ThenIGetTheRedirectView_AndTheNicknameIsSet()
        {
            UserViewModel userViewModel = new UserViewModel {IsLoggedIn = true, Nickname = "nickname"};

            _mockHttpContext.Setup(h => h.User).Returns(userViewModel);
            RedirectToRouteResult result = _controller.Login() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["nickname"], Is.EqualTo(userViewModel.Nickname));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Post").IgnoreCase);
        }
        
        [Test]
        public void GivenAnInvalidUser_WhenILogin_ThenIGetRedirectedToTheRegistrationPage()
        {
            _controller.ModelState.AddModelError("EMail", "Email error");

            ViewResult result = _controller.DoLogin(new UserViewModel()) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Login").IgnoreCase);
        }

        [Test]
        public void GivenAUserThatDoesNotExists_WhenTheyLogin_ThenIGetRedirectedToThePostsPage()
        {
            ViewResult result = _controller.DoLogin(new UserViewModel()) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Login").IgnoreCase);
        }

        [Test]
        public void GivenAUserThatAlreadyExists_WhenTheyLogin_ThenIGetRedirectedToThePostsPage()
        {
            string email = "email";
            string nickname = "nickname";
            User user = new User();
            user.AddUser("", email, "", false);
            _userRepository.Setup(u => u.GetUser(email)).Returns(user);
            
            RedirectToRouteResult result = _controller.DoLogin(new UserViewModel{Email = email}) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["Controller"], Is.EqualTo("Post").IgnoreCase);
            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index").IgnoreCase);
        }

        [Test]
        public void GivenAnInvalidUser_WhenTheyLogin_ThenIGetReturnedToTheLoginView()
        {
            string email = "email";
            string nickname = "nickname";
            User user = new User();
            user.AddUser("", email, "", false);
            
            _userRepository.Setup(u => u.GetUser(email)).Returns(user);

            ViewResult result = _controller.DoLogin(new UserViewModel { Nickname = nickname }) as ViewResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Login"));
        }

        [Test]
        public void GivenAnAuthenticatedUser_WhenIRegister_AndTheUserIsAlreadyAuthenticated_ThenIGetRedirected_AndTheNicknameIsSet()
        {
            UserViewModel userViewModel = new UserViewModel { IsLoggedIn = true, Nickname = "nickname" };

            _mockHttpContext.Setup(h => h.User).Returns(userViewModel);
            RedirectToRouteResult result = _controller.Register() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Post").IgnoreCase);
            Assert.That(result.RouteValues["nickname"], Is.EqualTo(userViewModel.Nickname));
        }

        [Test]
        public void GivenAnAuthenticatedUser_WhenIRegister_AndTheUserIsNotAuthenticated_ThenTheUserIsRegistered()
        {
            UserViewModel userViewModel = new UserViewModel { IsLoggedIn = false };

            _mockHttpContext.Setup(h => h.User).Returns(userViewModel);
            ViewResult result = _controller.Register() as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAnAuthenticatedUser_WhenIRegister_AndTheUserIsAlreadyRegistered_ThenIGetRedirectedToTheRegistrationPage()
        {
            string email = "foo@bar.com";
            UserViewModel userViewModel = new UserViewModel { Email = email};
            User user = new User();
            user.AddUser("", email, "", false);
            _userRepository.Setup(u => u.GetUser(email)).Returns(user);

            ViewResult result = _controller.DoRegister(userViewModel) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Register").IgnoreCase);
            Assert.That(_controller.ModelState["EMail"].Errors.Count, Is.EqualTo(1));
            Assert.That(_controller.ModelState["EMail"].Errors[0].ErrorMessage, Is.EqualTo("EMail already exists in database"));
        }

        [Test]
        public void GivenAnInvalidUser_WhenIRegister_ThenIGetRedirectedToTheRegistrationPage()
        {
            _controller.ModelState.AddModelError("EMail", "Email error");

            ViewResult result = _controller.DoRegister(new UserViewModel()) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Register").IgnoreCase);
        }

        [Test]
        public void GivenAValidUser_WhenIRegister_AndTheRegistrationIsSuccesful_ThenIGetRedirectedToTheHomePage()
        {
            RedirectToRouteResult result = _controller.DoRegister(new UserViewModel()) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Home").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index").IgnoreCase);
        }
        
        [Test]
        public void GivenALoggdInUser_WhenILogout_ThenTheCookieIsCleared()
        {
            var cookies = new HttpCookieCollection();
            string cookieName = "USER";

            _fakeResponse.Cookies.Add(new HttpCookie(cookieName));
            Assert.That(_fakeResponse.Cookies.Count, Is.EqualTo(1));

            cookies.Add(new HttpCookie(cookieName));
            _mockRequest.Setup(r => r.Cookies).Returns(cookies);

            RedirectToRouteResult result = _controller.Logout() as RedirectToRouteResult;
            Assert.That(_fakeResponse.Cookies.Count, Is.EqualTo(0));
        }

        [Test]
        public void GivenALoggdInUser_WhenILogout_ThenTheUserIsRemovedFromTheContext()
        {
            var cookies = new HttpCookieCollection();
            string cookieName = "USER";

            _controller.HttpContext.User = new UserViewModel();
            _mockRequest.Setup(r => r.Cookies).Returns(cookies);

            RedirectToRouteResult result = _controller.Logout() as RedirectToRouteResult;
            Assert.That(_controller.HttpContext.User, Is.Null);
        }
        [Test]
        public void GivenANotLoggdInUser_WhenILogout_TheUserIsRedirectedToTheHomePage()
        {
            var cookies = new HttpCookieCollection();
            _mockRequest.Setup(r => r.Cookies).Returns(cookies);
            RedirectToRouteResult result = _controller.Logout() as RedirectToRouteResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Home").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index").IgnoreCase);
        }
    }
}
