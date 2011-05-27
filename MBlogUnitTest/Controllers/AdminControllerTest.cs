using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models;
using MBlogModel;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    class AdminControllerTest : BaseControllerTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        //private IUserRepository _userRepository;

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserRepository.Setup(u => u.GetUserWithTheirBlogs(It.IsAny<int>())).Returns(new User{Blogs = new List<Blog>()});
        }

        [Test]
        public void GivenNoUserInContext_WhenIGoToTheAdminIndexPage_ThenIGetRedirectedToTheLoginPage()
        {
            AdminController controller = new AdminController(null);

            SetControllerContext(controller);

            RedirectToRouteResult result = controller.Index() as RedirectToRouteResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("User").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Login").IgnoreCase);
        }

        [Test]
        public void GivenAUserInContext_AndTheUserIsNotLoggedIn_WhenIGoToTheAdminIndexPage_ThenIGetRedirectedToTheLoginPage()
        {
            AdminController controller = new AdminController(null);

            SetControllerContext(controller);

            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = false };

            RedirectToRouteResult result = controller.Index() as RedirectToRouteResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("User").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Login").IgnoreCase);
        }

        [Test]
        public void GivenAUserInContext_AndTheUserIsLoggedIn_WhenIGoToTheAdminIndexPage_ThenIGetTheAdminPage()
        {
            AdminController controller = new AdminController(_mockUserRepository.Object);

            SetControllerContext(controller);

            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };

            ViewResult result = (ViewResult) controller.Index();
            Assert.That(result, Is.Not.Null);
        }
    }
}
