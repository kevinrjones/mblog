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
            headers.Add("HOST", "localhost");
            MockRequest.Setup(r => r.Headers).Returns(headers);
            MockHttpContext.Setup(h => h.Request).Returns(MockRequest.Object);

        }

        [Test]
        public void GivenABlogWithNoPosts_WhenIGetAFeed_ThenIGetNoItemsInTheFeed()
        {
            var controller = new FeedController(_mockBlogRepository.Object, _mockPostRepository.Object, null);
            SetControllerContext(controller);
            _mockBlogRepository.Setup(b => b.GetBlog(It.IsAny<string>())).Returns(new Blog {Description = "description", LastUpdated = new DateTime(), User = new User{Name = "name"}});
            _mockPostRepository.Setup(p => p.GetBlogPosts(It.IsAny<string>())).Returns(new List<Post>());
            var result = controller.Rss(nickname) as SyndicationActionResult;
            Assert.That(result.Feed, Is.Not.Null);
            Assert.That(result.Feed.Items.Count(), Is.EqualTo(0));
        }


        [Test]
        public void GivenABlogWithOnePost_WhenIGetAFeed_ThenIGetOneItemInTheFeed()
        {
            var controller = new FeedController(_mockBlogRepository.Object, _mockPostRepository.Object, null);
            SetControllerContext(controller);
            _mockBlogRepository.Setup(b => b.GetBlog(It.IsAny<string>())).Returns(new Blog { Description = "description", LastUpdated = new DateTime(), User = new User { Name = "name" } });
            _mockPostRepository.Setup(p => p.GetBlogPosts(It.IsAny<string>())).Returns(new List<Post>{new Post{Title = "title", BlogPost = "post"}});
            var result = controller.Rss(nickname) as SyndicationActionResult;
            Assert.That(result.Feed, Is.Not.Null);
            Assert.That(result.Feed.Items.Count(), Is.EqualTo(1));

        }
        [Test]
        public void GivenABlogWithOnePost_WhenIGetAFeed_ThenITheFeedItemHasTheCorrectValues()
        {
            var controller = new FeedController(_mockBlogRepository.Object, _mockPostRepository.Object, null);
            SetControllerContext(controller);
            _mockBlogRepository.Setup(b => b.GetBlog(It.IsAny<string>())).Returns(new Blog { Description = "description", LastUpdated = new DateTime(), User = new User { Name = "name" } });
            var post = new Post { Title = "title", BlogPost = "post" };
            _mockPostRepository.Setup(p => p.GetBlogPosts(It.IsAny<string>())).Returns(new List<Post> { post });

            var url = string.Format("http://localhost/{0}/{1}/{2}/{3}/{4}",  nickname, post.Posted.Year, post.Posted.Month, post.Posted.Day, post.TitleLink);

            var result = controller.Rss(nickname) as SyndicationActionResult;
            var item = result.Feed.Items.FirstOrDefault();

            Assert.That(item.Title.Text, Is.EqualTo(post.Title));
            Assert.That(((TextSyndicationContent)item.Content).Text, Is.EqualTo(post.BlogPost));
            Assert.That(item.PublishDate.DateTime, Is.EqualTo(post.Edited));
            Assert.That(item.Links[0].Uri, Is.EqualTo(new Uri(url)));
        }
    }
}
