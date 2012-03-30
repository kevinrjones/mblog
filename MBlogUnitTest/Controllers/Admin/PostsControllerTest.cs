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
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _postDomain = new Mock<IPostService>();
        }

        #endregion

        private Mock<IPostService> _postDomain;

        [Test]
        public void GivenAUserInContext_AndTheUserIsLoggedIn_WhenIListPosts_ThenIGetAllThePosts()
        {
            const int blogId = 1;
            const string nickname = "nickname";


            _postDomain.Setup(p => p.GetOrderedBlogPosts(It.IsAny<int>())).Returns(new List<Post>
                                                                                       {new Post {Title = "empty"}});

            var controller = new PostsController(_postDomain.Object, null);
            SetControllerContext(controller);

            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel {IsLoggedIn = true, Id = 1};

            var result = (ViewResult) controller.Index(new AdminBlogViewModel {Nickname = nickname, BlogId = blogId});
            var model = result.Model as PostsViewModel;
            Assert.That(model.Posts.Count, Is.EqualTo(1));
        }
    }
}