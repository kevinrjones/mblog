using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlogDomain;
using MBlogDomainInterfaces;
using MBlogModel;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Domain
{
    [TestFixture]
    public class PostDomainTest
    {
        IPostDomain _postDomain;
        private Mock<IPostRepository> _postRepository;

        [SetUp]
        public void Setup()
        {
            _postRepository = new Mock<IPostRepository>();
            _postDomain = new PostDomain(_postRepository.Object);
        }

        [Test]
        public void GivenAValidPost_WhenIAddAComment_ThenTheCommentIsWrittenToTheDatabase()
        {
            _postDomain.AddComment(1, "name", "comment");
            _postRepository.Verify(p => p.AddComment(1, "name", "comment"), Times.Once());
        }

        [Test]
        public void GivenAValidPost_WhenIAddAComment_AndTheDatabaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _postRepository.Setup(p => p.AddComment(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            Assert.Throws<MBlogException>(() => _postDomain.AddComment(1, "name", "comment"));
        }

        [Test]
        public void GivenAValidNickname_WhenThePostsAreRetrieved_ThenAllThePostsAreReturned()
        {
            _postRepository.Setup(p => p.GetBlogPosts("nickname")).Returns(new List<Post> {new Post()});
            var posts = _postDomain.GetBlogPosts("nickname");
            Assert.That(posts.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenAValidNickname_WhenThePostsAreRetrieved_AndTheDatabaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _postRepository.Setup(p => p.GetBlogPosts(It.IsAny<string>())).Throws<Exception>();
            Assert.Throws<MBlogException>(() => _postDomain.GetBlogPosts("nickname"));
        }

        [Test]
        public void WhenAllThePostsAreRetrieved_ThenAllThePostsAreReturned()
        {
            _postRepository.Setup(p => p.GetPosts()).Returns(new List<Post> { new Post() });
            var posts = _postDomain.GetBlogPosts();
            Assert.That(posts.Count, Is.EqualTo(1));
        }

        [Test]
        public void WhenAllThePostsAreRetrieved_AndTheDatabaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _postRepository.Setup(p => p.GetPosts()).Throws<Exception>();
            Assert.Throws<MBlogException>(() => _postDomain.GetBlogPosts());
        }

        [Test]
        public void GivenValidData_WhenThePostsAreRetrieved_ThenAllThePostsAreReturned()
        {
            _postRepository.Setup(p => p.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Post> { new Post() });
            var posts = _postDomain.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.That(posts.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenValidData_WhenThePostsAreRetrieved_AndTheDatabaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _postRepository.Setup(p => p.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            Assert.Throws<MBlogException>(() => _postDomain.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void GivenValidData_WhenAPostIsRetrieved_ThenThatPostIsReturned()
        {
            _postRepository.Setup(p => p.GetBlogPost(It.IsAny<int>())).Returns(new Post{Id = 1});
            var posts = _postDomain.GetBlogPost(It.IsAny<int>());
            Assert.That(posts.Id, Is.EqualTo(1));
        }

        [Test]
        public void GivenValidData_WhenAPostIsRetrieved_AndTheDatabaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _postRepository.Setup(p => p.GetBlogPost(It.IsAny<int>())).Throws<Exception>();
            Assert.Throws<MBlogException>(() => _postDomain.GetBlogPost(It.IsAny<int>()));
        }
    }
}
