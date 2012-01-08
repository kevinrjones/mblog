using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Mvc;
using MBlog.ActionResults;
using MBlog.Controllers;
using MBlog.Models;
using MBlog.Models.Admin;
using MBlog.Models.Post;
using MBlog.Models.User;
using MBlogDomainInterfaces;
using MBlogModel;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    class FeedControllerTest : BaseControllerTests
    {
        private Mock<ISyndicationFeedDomain> _syndicationFeedDomain;

        [SetUp]
        public void SetUp()
        {
            _syndicationFeedDomain = new Mock<ISyndicationFeedDomain>();
            MockRequest.Setup(r => r.Url).Returns(new Uri("http://foo.com/feed/rss"));
            var headers = new NameValueCollection();
            headers.Add("HOST", "localhost");
            MockRequest.Setup(r => r.Headers).Returns(headers);
            MockHttpContext.Setup(h => h.Request).Returns(MockRequest.Object);

        }

        [Test]
        public void WhenAnRssFeedIsRequested_ThenAnRssViewResultIsReturned()
        {
            var controller = new FeedController(_syndicationFeedDomain.Object, null);
            SetControllerContext(controller);
            _syndicationFeedDomain.Setup(
                s =>
                s.CreateSyndicationFeed(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new SyndicationFeed());
            SyndicationActionResult feed = (SyndicationActionResult) controller.Rss("kevin");
            FeedData feedData = feed.FeedData;
            Assert.That(feedData.ContentType, Is.EqualTo("application/rss+xml"));
        }

        [Test]
        public void WhenAnAtomFeedIsRequested_ThenAnRssViewResultIsReturned()
        {
            var controller = new FeedController(_syndicationFeedDomain.Object, null);
            SetControllerContext(controller);
            _syndicationFeedDomain.Setup(
                s =>
                s.CreateSyndicationFeed(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new SyndicationFeed());
            SyndicationActionResult feed = (SyndicationActionResult)controller.Atom("kevin");
            FeedData feedData = feed.FeedData;
            Assert.That(feedData.ContentType, Is.EqualTo("application/atom+xml"));
        }

    }
}
