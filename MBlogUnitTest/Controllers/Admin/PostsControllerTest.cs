using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MBlog.Controllers.Admin;
using MBlog.Models.Admin;
using MBlog.Models.Post;
using MBlog.Models.User;
using MBlogDomainInterfaces;
using MBlogModel;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers.Admin
{
    [TestFixture]
    class PostsControllerTest : BaseControllerTests
    {
        private Mock<IPostDomain> _postDomain;

        [SetUp]
        public void Setup()
        {
            _postDomain = new Mock<IPostDomain>();
        }

        [Test]
        public void GivenAUserInContext_AndTheUserIsLoggedIn_WhenIListPosts_ThenIGetAllThePosts()
        {
            const int blogId = 1;
            const string nickname = "nickname";

            
            _postDomain.Setup(p => p.GetOrderedBlogPosts(It.IsAny<int>())).Returns(new List<Post> { new Post { Title = "empty" } });
        
            var controller = new PostsController(_postDomain.Object, null);
            SetControllerContext(controller);

            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };

            var result = (ViewResult)controller.Index(new AdminBlogViewModel { Nickname = nickname, BlogId = blogId });
            var model = result.Model as PostsViewModel;
            Assert.That(model.Posts.Count, Is.EqualTo(1));
        }

    }
}
