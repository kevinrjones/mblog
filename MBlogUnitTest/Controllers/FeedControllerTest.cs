using System;
using System.Collections.Specialized;
using System.ServiceModel.Syndication;
using MBlog.ActionResults;
using MBlog.Controllers;
using MBlogServiceInterfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    internal class FeedControllerTest : BaseControllerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _syndicationFeedDomain = new Mock<ISyndicationFeedService>();
            MockRequest.Setup(r => r.Url).Returns(new Uri("http://foo.com/feed/rss"));
            var headers = new NameValueCollection();
            headers.Add("HOST", "localhost");
            MockRequest.Setup(r => r.Headers).Returns(headers);
            MockHttpContext.Setup(h => h.Request).Returns(MockRequest.Object);
        }

        #endregion

        private Mock<ISyndicationFeedService> _syndicationFeedDomain;

        [Test]
        public void WhenAnAtomFeedIsRequested_ThenAnRssViewResultIsReturned()
        {
            var controller = new FeedController(_syndicationFeedDomain.Object, null);
            SetControllerContext(controller);
            _syndicationFeedDomain.Setup(
                s =>
                s.CreateSyndicationFeed(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new SyndicationFeed());
            var feed = (SyndicationActionResult) controller.Atom("kevin");
            FeedData feedData = feed.FeedData;
            Assert.That(feedData.ContentType, Is.EqualTo("application/atom+xml"));
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
            var feed = (SyndicationActionResult) controller.Rss("kevin");
            FeedData feedData = feed.FeedData;
            Assert.That(feedData.ContentType, Is.EqualTo("application/rss+xml"));
        }
    }
}