using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models;
using MBlogModel;
using MBlogRepository;
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
            mock.Setup(r => r.GetBlogPosts("")).Returns(posts);

            _repository = mock.Object;
        }

        [Test]
        public void GivenAPostController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectView()
        {
            PostController controller = new PostController(_repository, null);
            ActionResult result = controller.Index("");

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
    }
}
