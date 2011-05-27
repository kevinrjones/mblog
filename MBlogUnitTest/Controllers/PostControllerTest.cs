using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models;
using MBlogModel;
using MBlogRepository;
using MBlogRepository.Interfaces;
using MBlogRepository.Repositories;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    public class TestPostController : BaseControllerTests
    {
        private IPostRepository _postRepositoryMock;
        private IBlogRepository _blogRepositoryMock;

        List<Post> posts = new List<Post> { new Post { Title = "title1" }, new Post { Title = "title2" }, new Post { Title = "title3" } };
        [SetUp]
        public void SetUp()
        {
            var blogPostRepositoryMock = new Mock<IPostRepository>();
            _postRepositoryMock = blogPostRepositoryMock.Object;

            blogPostRepositoryMock.Setup(r => r.GetBlogPosts(It.IsAny<string>())).Returns(posts);
            blogPostRepositoryMock.Setup(r => r.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Post> { new Post { Id = 1, BlogPost = "empty", Title = "empty", Posted = DateTime.Today, Comments = new List<Comment> { new Comment { CommentText = "empty", Approved = true }, new Comment { CommentText = "empty", Approved = false } } } });

            var mockBlog = new Mock<IBlogRepository>();
            _blogRepositoryMock = mockBlog.Object;

            mockBlog.Setup(b => b.GetBlog(It.IsAny<string>())).Returns(new Blog());
        }

        [Test]
        public void GivenAPostController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectView()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, null);
            ViewResult result = (ViewResult)controller.Index("");

            PostsViewModel model = (PostsViewModel)result.Model;

            Assert.That(model.Posts.Count(), Is.EqualTo(posts.Count));
        }

        [Test]
        public void GivenAPostController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectNumberOfPosts()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, null);
            ViewResult result = (ViewResult)controller.Index("");

            PostsViewModel model = (PostsViewModel)result.Model;

            Assert.That(model.Posts.Count(), Is.EqualTo(posts.Count));
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_ThenItReturnsTheCorrectView()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, null);
            ViewResult result = controller.Show(new PostLinkViewModel()) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_ThenItReturnsTheCorrectModelInTheView()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, null);
            ViewResult result = controller.Show(new PostLinkViewModel()) as ViewResult;

            Assert.That(result.Model, Is.AssignableTo<PostsViewModel>());
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_AndIPassAYear_ThenItReturnsTheCorrectPosts()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, null);
            ViewResult result = controller.Show(new PostLinkViewModel { Year = 2010, Month = 0, Day = 0, Link = null }) as ViewResult;
            PostsViewModel model = (PostsViewModel)result.Model;

            Assert.That(model.Posts.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenAPostController_WhenIGetItsPostsWithComments_ThenItReturnsTheCorrectComments()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, null);
            ViewResult result = controller.Show(new PostLinkViewModel()) as ViewResult;
            PostsViewModel model = (PostsViewModel)result.Model;

            Assert.That(model.Posts.Count, Is.EqualTo(1));
            Assert.That(model.Posts[0].Comments.Count, Is.EqualTo(1));
        }
    }
}
