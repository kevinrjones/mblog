using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlogModel;
using MBlogRepository;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest
{
    [TestFixture]
    public class TestPostController
    {
        private IBlogPostRepository _repository;
        [SetUp]
        public void SetUp()
        {
            var mock = new Mock<IBlogPostRepository>();
            //mock.Setup(r => r.GetBlogPosts()).Returns(new List<Post> {});

            _repository = mock.Object;
        }

        [Test]
        public void GivenAPostController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectView()
        {
            PostController controller = new PostController(_repository);
            ActionResult result = controller.Index();

            Assert.That(result, Is.TypeOf(typeof(ViewResult)));
        }
    }
}
