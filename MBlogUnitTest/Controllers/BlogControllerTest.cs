using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models.Blog;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    public class BlogControllerTest : BaseControllerTests
    {
        private Mock<IBlogRepository> _blogRepository;
        private Mock<IUserRepository> _userRepository;
        private BlogController _controller;

        [SetUp]
        public void SetUp()
        {
            _blogRepository = new Mock<IBlogRepository>();
            _userRepository = new Mock<IUserRepository>();
            _controller = new BlogController(_userRepository.Object, _blogRepository.Object);
        }

        [Test]
        public void GivenNoLoggedInUser_WhenNewIsCalled_ThenTheRequestIsRedirected()
        {
            MockHttpContext.SetupProperty(h => h.User);

            SetControllerContext(_controller);
            _controller.HttpContext.User = new UserViewModel { IsLoggedIn = false };

            var result = _controller.New();
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
        }

        [Test]
        public void GivenALoggedInUser_WhenNewIsCalled_ThenTheNewViewIsReturned()
        {
            MockHttpContext.SetupProperty(h => h.User);

            SetControllerContext(_controller);
            _controller.HttpContext.User = new UserViewModel { IsLoggedIn = true };

            var result = _controller.New();
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void GivenNoLoggedInUser_WhenCreateIsCalled_ThenTheRequestIsRedirected()
        {
            MockHttpContext.SetupProperty(h => h.User);

            SetControllerContext(_controller);
            _controller.HttpContext.User = new UserViewModel { IsLoggedIn = false };

            var result = _controller.Create(new CreateBlogViewModel());
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
        }

        [Test]
        public void GivenALoggedInUser_WhenCreateIsCalled_AndTheModelIsInvalid_ThenTheNewViewIsReturned()
        {
            MockHttpContext.SetupProperty(h => h.User);

            SetControllerContext(_controller);
            _controller.HttpContext.User = new UserViewModel { IsLoggedIn = true };
            _controller.ModelState.AddModelError("error", "message");

            ViewResult result = _controller.Create(new CreateBlogViewModel()) as ViewResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("New").IgnoreCase);

        }

        [Test]
        public void GivenALoggedInUser_WhenCreateIsCalled_AndTheModelIsValid_ThenTheAdminPageIsReturned()
        {
            MockHttpContext.SetupProperty(h => h.User);

            SetControllerContext(_controller);
            _controller.HttpContext.User = new UserViewModel { IsLoggedIn = true };

            RedirectToRouteResult result = _controller.Create(new CreateBlogViewModel()) as RedirectToRouteResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("dashboard").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index").IgnoreCase);

        }

        [Test]
        public void GivenALoggedInUser_WhenITryAndEditABlog_ThenIGetTheCorrectView()
        {
            string nickname = "nickname";
            _blogRepository.Setup(b => b.GetBlog(nickname)).Returns(new Blog("title", "description", true, true,
                                                                               nickname, 1));
            var controller = new BlogController(_userRepository.Object, _blogRepository.Object);

            var result = controller.Edit(new CreateBlogViewModel{Nickname = nickname});

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void GivenAValidModel_WhenITryAndUpdateABlog_ThenIGetTheCorrectView()
        {
            string nickname = "nickname";
            _blogRepository.Setup(b => b.GetBlog(nickname)).Returns(new Blog("title", "description", true, true,
                                                                               nickname, 1));
            var controller = new BlogController(_userRepository.Object, _blogRepository.Object);


            RedirectToRouteResult result = controller.Update(new CreateBlogViewModel { Nickname = nickname, Description = "desc", Title = "title"}) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Dashboard").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("index").IgnoreCase);
        }

        [Test]
        public void GivenAnInvalidModel_WhenITryAndUpdateABlog_ThenIHaveToReEditTheBlogData()
        {
            string nickname = "nickname";
            _blogRepository.Setup(b => b.GetBlog(nickname)).Returns(new Blog("title", "description", true, true,
                                                                               nickname, 1));
            var controller = new BlogController(_userRepository.Object, _blogRepository.Object);

            controller.ModelState.AddModelError("Name", "Name error");

            var result = controller.Update(new CreateBlogViewModel ()) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }


    }
}
