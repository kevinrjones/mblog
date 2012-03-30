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
using MBlogService;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    class SyndicationFeedDomainTest : BaseControllerTests
    {
        Mock<IBlogRepository> _blogRepository;
        Mock<IPostRepository> _postRepository;

        [SetUp]
        public void Setup()
        {
            _blogRepository = new Mock<IBlogRepository>();
            _postRepository = new Mock<IPostRepository>();

            _blogRepository.Setup(b => b.GetBlog("nickname")).Returns(new Blog { Title = "title", Description = "description", LastUpdated = DateTime.UtcNow, User = new User { Name = "name" } });
        }

        [Test]
        public void GivenThreePosts_TheAllPostsAppearInTheFeed()
        {
            _postRepository.Setup(p => p.GetBlogPosts("nickname")).Returns(new List<Post> { new Post(), new Post(), new Post() });
            SyndicationFeedService feedService = new SyndicationFeedService(_blogRepository.Object, _postRepository.Object);
            var syndicationFeed = feedService.CreateSyndicationFeed("nickname", "feedtype", "scheme", "host");
            Assert.That(syndicationFeed.Items.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GivenAPost_TheTheItemContainsTheCorrectData()
        {
            _postRepository.Setup(p => p.GetBlogPosts("nickname")).Returns(new List<Post> { new Post { Title = "postTitle", BlogPost = "body", Edited = new DateTime(2010, 1, 1) } });
            SyndicationFeedService feedService = new SyndicationFeedService(_blogRepository.Object, _postRepository.Object);
            var syndicationFeed = feedService.CreateSyndicationFeed("nickname", "feedtype", "scheme", "host");
            var item = syndicationFeed.Items.FirstOrDefault();
            var content = (TextSyndicationContent)item.Content;
            Assert.That(item.Title.Text, Is.EqualTo("postTitle"));
            Assert.That(content.Text, Is.EqualTo("body"));
            Assert.That(item.PublishDate.DateTime, Is.EqualTo(new DateTime(2010, 1, 1)));
        }

    }
}
