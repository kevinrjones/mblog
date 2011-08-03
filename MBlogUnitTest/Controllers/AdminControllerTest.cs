using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models;
using MBlog.Models.Admin;
using MBlog.Models.Post;
using MBlog.Models.User;
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

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserRepository.Setup(u => u.GetUserWithTheirBlogs(It.IsAny<int>())).Returns(new User{Blogs = new List<Blog>{new Blog()}});
        }

        [Test]
        public void GivenAUserInContext_AndTheUserIsLoggedIn_WhenIGoToTheAdminIndexPage_ThenIGetTheAdminPage()
        {
            AdminController controller = new AdminController(_mockUserRepository.Object, null, null);

            SetControllerContext(controller);

            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };

            ViewResult result = (ViewResult)controller.Index();
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAUserInContext_AndTheUserIsLoggedIn_WhenIGoToTheAdminIndexPage_ThenIGetAllTheBlogs()
        {
            AdminController controller = new AdminController(_mockUserRepository.Object, null, null);

            SetControllerContext(controller);

            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };

            ViewResult result = (ViewResult)controller.Index();
            AdminUserViewModel model = result.Model as AdminUserViewModel;
            Assert.That(model.Blogs.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenAUserInContext_AndTheUserIsLoggedIn_WhenIListPosts_ThenIGetAllThePosts()
        {
            var blogRepository = new Mock<IBlogRepository>();
            int blogId = 1;
            string nickname = "nickname";

            blogRepository.Setup(b => b.GetBlog(It.IsAny<string>())).Returns(new Blog{Nickname = nickname, Id = blogId});
            var postRepository = new Mock<IPostRepository>();
            postRepository.Setup(p => p.GetOrderedBlogPosts(It.IsAny<int>())).Returns(new List<Post>{new Post{Title = "empty"}});
            AdminController controller = new AdminController(_mockUserRepository.Object, postRepository.Object, blogRepository.Object);

            SetControllerContext(controller);

            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };

            ViewResult result = (ViewResult)controller.ListPosts(new AdminBlogViewModel{Nickname = nickname, BlogId = blogId});
            PostsViewModel model = result.Model as PostsViewModel;
            Assert.That(model.Posts.Count, Is.EqualTo(1));
        }
    }
}
