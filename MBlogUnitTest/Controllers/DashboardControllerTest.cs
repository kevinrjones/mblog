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
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _userDomain = new Mock<IUserService>();
            _postDomain = new Mock<IPostService>();
            _userDomain.Setup(u => u.GetUserWithTheirBlogs(It.IsAny<int>())).Returns(new User
                                                                                         {
                                                                                             Blogs =
                                                                                                 new List<Blog>
                                                                                                     {new Blog()}
                                                                                         });
            _controller = new DashboardController(_postDomain.Object, _userDomain.Object, null);
        }

        #endregion

        private Mock<IUserService> _userDomain;
        private Mock<IPostService> _postDomain;
        private DashboardController _controller;

        [Test]
        public void GivenAUserInContext_AndTheUserIsLoggedIn_WhenIGoToTheAdminIndexPage_ThenIGetAllTheBlogs()
        {
            SetControllerContext(_controller);

            MockHttpContext.SetupProperty(h => h.User);
            _controller.HttpContext.User = new UserViewModel {IsLoggedIn = true, Id = 1};

            var result = (ViewResult) _controller.Index();
            var model = (AdminUserViewModel) result.Model;

            Assert.That(model.Blogs.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenAUserInContext_AndTheUserIsLoggedIn_WhenIGoToTheAdminIndexPage_ThenIGetTheAdminPage()
        {
            SetControllerContext(_controller);

            MockHttpContext.SetupProperty(h => h.User);
            _controller.HttpContext.User = new UserViewModel {IsLoggedIn = true, Id = 1};

            var result = (ViewResult) _controller.Index();
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAUserInContext_AndTheUserIsLoggedIn_WhenIListAllThePosts_ThenIGetThePosts()
        {
            SetControllerContext(_controller);

            MockHttpContext.SetupProperty(h => h.User);
            _controller.HttpContext.User = new UserViewModel {IsLoggedIn = true, Id = 1};
            _postDomain.Setup(p => p.GetOrderedBlogPosts(1)).Returns(new List<Post> {new Post()});
            var result = (ViewResult) _controller.ListPosts(new AdminBlogViewModel {BlogId = 1});
            var model = (PostsViewModel) result.Model;

            Assert.That(model.Posts.Count, Is.EqualTo(1));
        }
    }
}