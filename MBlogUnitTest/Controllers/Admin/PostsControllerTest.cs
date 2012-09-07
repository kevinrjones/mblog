using System.Collections.Generic;
using System.Web.Mvc;
using MBlog.Controllers.Admin;
using MBlog.Models.Admin;
using MBlog.Models.Post;
using MBlog.Models.User;
using MBlogModel;
using MBlogServiceInterfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers.Admin
{
    [TestFixture]
    internal class PostsControllerTest : BaseControllerTests
    {
        private Mock<IPostService> _postService;
        private Mock<IBlogService> _blogService;

        [SetUp]
        public void Setup()
        {
            _postService = new Mock<IPostService>();
            _blogService = new Mock<IBlogService>();
        }


        [Test]
        public void GivenAUserInContext_AndTheUserIsLoggedIn_WhenIListPosts_ThenIGetAllThePosts()
        {
            const string nickname = "nickname";


            _postService.Setup(p => p.GetOrderedBlogPosts(It.IsAny<int>())).Returns(new List<Post>
                                                                                       {new Post {Title = "empty"}});
            _blogService.Setup(b => b.GetBlog(nickname)).Returns(new Blog{Id = 1});
            var controller = new PostsController(_postService.Object, _blogService.Object, null);
            SetControllerContext(controller);

            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel {IsLoggedIn = true, Id = 1};

            var result = (ViewResult) controller.Index(new AdminBlogViewModel {Nickname = nickname});
            var model = result.Model as PostsViewModel;
            Assert.That(model.Posts.Count, Is.EqualTo(1));
        }
    }
}