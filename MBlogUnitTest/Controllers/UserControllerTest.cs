using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Filters;
using MBlog.Infrastructure;
using MBlog.Models;
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

            _controller = new UserController(_userRepository.Object, _usernameBlacklistRepository.Object);

            SetControllerContext(_controller);
        }

        [Test]
        public void GivenNoAuthenticatedUser_WhenILogin_ThenIGetTheLoginView()
        {
            MockHttpContext.Setup(h => h.User).Returns(new UserViewModel());
            ActionResult result = _controller.Login();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void GivenAnAuthenticatedUser_WhenILogin_ThenIGetTheRedirectView()
        {
            UserViewModel userViewModel = new UserViewModel {IsLoggedIn = true};

            MockHttpContext.Setup(h => h.User).Returns(userViewModel);
            RedirectToRouteResult result = _controller.Login() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index").IgnoreCase);
        }
        
        [Test]
        public void GivenAnInvalidEMail_WhenILogin_ThenIGetRedirectedToTheRegistrationPage()
        {
            _controller.ModelState.AddModelError("EMail", "Email error");

            ViewResult result = _controller.DoLogin(new LoginUserViewModel()) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Login").IgnoreCase);
        }

        [Test]
        public void GivenANonMatchingPassword_WhenILogin_ThenIGetRedirectedToTheRegistrationPage()
        {
            _userRepository.Setup(u => u.GetUser("email@mail.com")).Returns(new User{Password = "foo"});
            ViewResult result = _controller.DoLogin(new LoginUserViewModel { Email = "email@mail.com", Password = "password" }) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Login").IgnoreCase);
        }

        [Test]
        public void GivenAMatchingPassword_WhenILogin_ThenIGetRedirectedToTheAdminPage()
        {
            string password = "password";
            _userRepository.Setup(u => u.GetUser("email@mail.com")).Returns(new User { Password = password });
            RedirectToRouteResult result = _controller.DoLogin(new LoginUserViewModel { Email = "email@mail.com", Password = password }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("admin"));
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public void GivenAUserThatDoesNotExists_WhenTheyLogin_ThenIGetRedirectedToThePostsPage()
        {
            ViewResult result = _controller.DoLogin(new LoginUserViewModel()) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Login").IgnoreCase);
        }

        [Test]
        public void GivenAUserThatExists_WhenTheyLogin_ThenIGetRedirectedToTheAdminPage()
        {
            string email = "email";
            User user = new User();
            user.AddUserDetails("", email, "", false);
            _userRepository.Setup(u => u.GetUser(email)).Returns(user);

            RedirectToRouteResult result = _controller.DoLogin(new LoginUserViewModel { Email = email }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["Controller"], Is.EqualTo("Admin").IgnoreCase);
            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index").IgnoreCase);
        }

        [Test]
        public void GivenAUserThatExists_WhenTheyLogin_ThenTheUserIsInTheContext()
        {
            string email = "email";
            User user = new User();
            user.AddUserDetails("", email, "", false);
            _userRepository.Setup(u => u.GetUser(email)).Returns(user);

            MockHttpContext.SetupProperty(h => h.User);

            Assert.That(_controller.HttpContext.User, Is.Null);
            _controller.DoLogin(new LoginUserViewModel { Email = email });

            Assert.That(_controller.HttpContext.User, Is.Not.Null);
        }

        [Test]
        public void GivenAUserThatExists_WhenTheyLogin_ThenTheCorrectCookieIsSet()
        {
            string email = "email";
            User user = new User{Id = 1};
            user.AddUserDetails("", email, "", false);
            _userRepository.Setup(u => u.GetUser(email)).Returns(user);

            MockHttpContext.SetupProperty(h => h.User);

            Assert.That(_controller.HttpContext.User, Is.Null);
            _controller.DoLogin(new LoginUserViewModel { Email = email });

            byte[] cipherText = user.Id.ToString().Encrypt();
            string base64CipherText = Convert.ToBase64String(cipherText);


            Assert.That(FakeResponse.Cookies.Count, Is.EqualTo(1));
            HttpCookie cookie = FakeResponse.Cookies[0];
            Assert.That(cookie.Value, Is.EqualTo(base64CipherText));
        }

        [Test]
        public void GivenAUserThatExists_WhenTheyLogin_ThenTheUserMarkedAsLoggedIn()
        {
            string email = "email";
            User user = new User();
            user.AddUserDetails("", email, "", false);
            _userRepository.Setup(u => u.GetUser(email)).Returns(user);

            MockHttpContext.SetupProperty(h => h.User);

            Assert.That(_controller.HttpContext.User, Is.Null);
            _controller.DoLogin(new LoginUserViewModel { Email = email });

            UserViewModel model = _controller.HttpContext.User as UserViewModel;
            Assert.That(model.IsAuthenticated, Is.True);
        }

        [Test]
        public void GivenAnInvalidUser_WhenTheyLogin_ThenIGetReturnedToTheLoginView()
        {
            string email = "email";
            User user = new User();
            user.AddUserDetails("", email, "", false);
            
            _userRepository.Setup(u => u.GetUser(email)).Returns(user);

            ViewResult result = _controller.DoLogin(new LoginUserViewModel ()) as ViewResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Login"));
        }

        [Test]
        public void GivenAUser_WhenIRegister_AndTheUserIsAlreadyAuthenticated_ThenIGetRedirectedToTheAdminPage()
        {
            UserViewModel userViewModel = new UserViewModel { IsLoggedIn = true};

            MockHttpContext.Setup(h => h.User).Returns(userViewModel);
            RedirectToRouteResult result = _controller.Register() as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("admin").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("index").IgnoreCase);
        }

        [Test]
        public void GivenAUser_WhenIRegister_AndTheUserIsNotLoggdIn_ThenTheUserIsSentToTheRegistrationPage()
        {
            UserViewModel userViewModel = new UserViewModel { IsLoggedIn = false };

            MockHttpContext.Setup(h => h.User).Returns(userViewModel);
            ViewResult result = _controller.Register() as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAUser_WhenIRegister_AndTheUserIsAlreadyRegistered_ThenIGetRedirectedToTheRegistrationPage()
        {
            string email = "foo@bar.com";
            UserViewModel userViewModel = new UserViewModel { Email = email};
            User user = new User();
            user.AddUserDetails("", email, "", false);
            _userRepository.Setup(u => u.GetUser(email)).Returns(user);

            ViewResult result = _controller.DoRegister(userViewModel) as ViewResult;

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

            ViewResult result = _controller.DoRegister(userViewModel) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Register").IgnoreCase);
            Assert.That(_controller.ModelState["Name"].Errors.Count, Is.EqualTo(1));
            Assert.That(_controller.ModelState["Name"].Errors[0].ErrorMessage, Is.EqualTo("That user name is reserved"));
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
        public void GivenAValidUser_WhenIRegister_AndTheRegistrationIsSuccesful_ThenIGetRedirectedToTheAdminPage()
        {
            RedirectToRouteResult result = _controller.DoRegister(new UserViewModel()) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Admin").IgnoreCase);
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
            _controller.HttpContext.User = new UserViewModel();

            Assert.That(_controller.HttpContext.User, Is.Not.Null);
            RedirectToRouteResult result = _controller.Logout() as RedirectToRouteResult;
            Assert.That(_controller.HttpContext.User, Is.Null);
        }
        [Test]
        public void GivenANotLoggdInUser_WhenILogout_TheUserIsRedirectedToTheHomePage()
        {
            var cookies = new HttpCookieCollection();
            MockRequest.Setup(r => r.Cookies).Returns(cookies);
            RedirectToRouteResult result = _controller.Logout() as RedirectToRouteResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Home").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index").IgnoreCase);
        }
    }
}
