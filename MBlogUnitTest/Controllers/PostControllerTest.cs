using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models;
using MBlogModel;
using MBlogRepository;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    public class TestPostController
    {
        private IBlogPostRepository _repository;
        List<Post> posts = new List<Post>{new Post(), new Post(), new Post()};
        [SetUp]
        public void SetUp()
        {
            var mock = new Mock<IBlogPostRepository>();
            mock.Setup(r => r.GetBlogPosts(It.IsAny<string>())).Returns(posts);
            mock.Setup(r => r.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(new List<Post>{new Post{Id = 1, BlogPost = "empty", Title = "empty", Posted = DateTime.Today}});
            _repository = mock.Object;
        }

        [Test]
        public void GivenAPostController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectView()
        {
            PostController controller = new PostController(_repository, null);
            ActionResult result = controller.Index(It.IsAny<string>());

            Assert.That(result, Is.TypeOf(typeof(ViewResult)));
        }

        [Test]
        public void GivenAPostController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectNumberOfPosts()
        {
            PostController controller = new PostController(_repository, null);
            ViewResult result = (ViewResult) controller.Index("");

            IEnumerable<PostViewModel> model = (IEnumerable<PostViewModel>) result.Model;

            Assert.That(model.Count(), Is.EqualTo(posts.Count));
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_ThenItReturnsTheCorrectView()
        {
            PostController controller = new PostController(_repository, null);
            ViewResult result = controller.Show(new PostLinkViewModel()) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_ThenItReturnsTheCorrectModelInTheView()
        {
            PostController controller = new PostController(_repository, null);
            ViewResult result = controller.Show(new PostLinkViewModel()) as ViewResult;

            Assert.That(result.Model, Is.AssignableTo<IEnumerable<PostViewModel>>());
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_AndIPassAYear_ThenItReturnsTheCorrectPosts()
        {
            PostController controller = new PostController(_repository, null);
            ViewResult result = controller.Show(new PostLinkViewModel()) as ViewResult;
            IEnumerable<PostViewModel> posts = (IEnumerable<PostViewModel>) result.Model;

            Assert.That(posts.Count(), Is.EqualTo(1));
        }

    }
}
