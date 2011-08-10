using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MBlog.ActionResults;
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
    class FeedControllerTest : BaseControllerTests
    {
        private Mock<IPostRepository> _mockPostRepository;
        private Mock<IBlogRepository> _mockBlogRepository;
        String nickname;

        [SetUp]
        public void SetUp()
        {
            nickname = "nickname";
            _mockPostRepository = new Mock<IPostRepository>();
            _mockBlogRepository = new Mock<IBlogRepository>();
            MockRequest.Setup(r => r.Url).Returns(new Uri("http://foo.com/feed/rss"));
            var headers = new NameValueCollection();
            headers.Add("HOST", "foo");
            MockRequest.Setup(r => r.Headers).Returns(headers);
            MockHttpContext.Setup(h => h.Request).Returns(MockRequest.Object);

        }

        [Test]
        public void GivenABlogWithNoPosts_WhenIetAFeed_ThenIGetNoItemsInTheFeed()
        {
            var controller = new FeedController(_mockBlogRepository.Object, _mockPostRepository.Object, null);
            SetControllerContext(controller);
            var result = controller.Rss(nickname) as SyndicationActionResult;
            Assert.That(result.Feed, Is.Not.Null);
        }
    }
}
