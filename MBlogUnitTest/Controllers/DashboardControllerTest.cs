using System.Collections.Generic;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models.Admin;
using MBlog.Models.Post;
using MBlog.Models.User;
using MBlogModel;
using MBlogServiceInterfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    internal class DashboardControllerTest : BaseControllerTests
    {
        private Mock<IUserService> _userService;
        private Mock<IPostService> _postService;
        private Mock<IBlogService> _blogService;
        private DashboardController _controller;


        [SetUp]
        public void SetUp()
        {
            _userService = new Mock<IUserService>();
            _postService = new Mock<IPostService>();
            _userService.Setup(u => u.GetUserWithTheirBlogs(It.IsAny<int>())).Returns(new User
                                                                                         {
                                                                                             Blogs =
                                                                                                 new List<Blog> { new Blog() }

                                                                                         });
            _blogService = new Mock<IBlogService>();
            _controller = new DashboardController(_postService.Object, _userService.Object, _blogService.Object, null);
        }

        [Test]
        public void GivenAUserInContext_AndTheUserIsLoggedIn_WhenIGoToTheAdminIndexPage_ThenIGetAllTheBlogs()
        {
            SetControllerContext(_controller);

            MockHttpContext.SetupProperty(h => h.User);
            _controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };

            var result = (ViewResult)_controller.Index();
            var model = (AdminUserViewModel)result.Model;

            Assert.That(model.Blogs.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenAUserInContext_AndTheUserIsLoggedIn_WhenIGoToTheAdminIndexPage_ThenIGetTheAdminPage()
        {
            SetControllerContext(_controller);

            MockHttpContext.SetupProperty(h => h.User);
            _controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };

            var result = (ViewResult)_controller.Index();
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAUserInContext_AndTheUserIsLoggedIn_WhenIListAllThePosts_ThenIGetThePosts()
        {
            SetControllerContext(_controller);

            MockHttpContext.SetupProperty(h => h.User);
            _controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };
            _blogService.Setup(b => b.GetBlog(It.IsAny<string>())).Returns(new Blog{Id = 1});
            _postService.Setup(p => p.GetOrderedBlogPosts(1)).Returns(new List<Post> { new Post() });
            var result = (ViewResult)_controller.ListPosts(new AdminBlogViewModel());
            var model = (PostsViewModel)result.Model;

            Assert.That(model.Posts.Count, Is.EqualTo(1));
        }
    }
}