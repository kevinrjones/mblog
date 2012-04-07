using System;
using System.Collections.Generic;
using MBlogModel;
using MBlogRepository.Interfaces;
using MBlogService;
using MBlogServiceInterfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Services
{
    [TestFixture]
    public class PostServiceTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _postRepository = new Mock<IPostRepository>();
            _postService = new PostService(_postRepository.Object);
        }

        #endregion

        private IPostService _postService;
        private Mock<IPostRepository> _postRepository;

        [Test]
        public void GivenABlogId_WhenIAskForOrderedBlogPosts_AndTheDatabaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _postRepository.Setup(p => p.GetOrderedBlogPosts(It.IsAny<int>())).Throws<Exception>();
            Assert.Throws<MBlogException>(() => _postService.GetOrderedBlogPosts(1));
        }

        [Test]
        public void GivenABlogId_WhenIAskForOrderedBlogPosts_ThenThePostsAreReturned()
        {
            _postRepository.Setup(p => p.GetOrderedBlogPosts(1)).Returns(new List<Post> {new Post()});
            IList<Post> posts = _postService.GetOrderedBlogPosts(1);
            Assert.That(posts.Count, Is.EqualTo(1));
        }

        [Test]
        public void
            GivenAValidNickname_WhenThePostsAreRetrieved_AndTheDatabaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _postRepository.Setup(p => p.GetBlogPosts(It.IsAny<string>())).Throws<Exception>();
            Assert.Throws<MBlogException>(() => _postService.GetBlogPosts("nickname"));
        }

        [Test]
        public void GivenAValidNickname_WhenThePostsAreRetrieved_ThenAllThePostsAreReturned()
        {
            _postRepository.Setup(p => p.GetBlogPosts("nickname")).Returns(new List<Post> {new Post()});
            IList<Post> posts = _postService.GetBlogPosts("nickname");
            Assert.That(posts.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenAValidPost_WhenIAddAComment_AndTheDatabaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _postRepository.Setup(p => p.AddComment(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Throws
                <Exception>();
            Assert.Throws<MBlogException>(() => _postService.AddComment(1, "name", "comment"));
        }

        [Test]
        public void GivenAValidPost_WhenIAddAComment_ThenTheCommentIsWrittenToTheDatabase()
        {
            _postService.AddComment(1, "name", "comment");
            _postRepository.Verify(p => p.AddComment(1, "name", "comment"), Times.Once());
        }

        [Test]
        public void GivenValidData_WhenAPostIsDeleted_AndTheDatabaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _postRepository.Setup(p => p.Delete(It.IsAny<Post>())).Throws<Exception>();
            Assert.Throws<MBlogException>(() => _postService.Delete(It.IsAny<int>()));
        }

        [Test]
        public void GivenValidData_WhenAPostIsDeleted_ThenThatPostIsReturned()
        {
            _postService.Delete(It.IsAny<int>());
            _postRepository.Verify(p => p.Delete(It.IsAny<Post>()), Times.Once());
        }

        [Test]
        public void GivenValidData_WhenAPostIsRetrieved_AndTheDatabaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _postRepository.Setup(p => p.GetBlogPost(It.IsAny<int>())).Throws<Exception>();
            Assert.Throws<MBlogException>(() => _postService.GetBlogPost(It.IsAny<int>()));
        }

        [Test]
        public void GivenValidData_WhenAPostIsRetrieved_ThenThatPostIsReturned()
        {
            _postRepository.Setup(p => p.GetBlogPost(It.IsAny<int>())).Returns(new Post {Id = 1});
            Post posts = _postService.GetBlogPost(It.IsAny<int>());
            Assert.That(posts.Id, Is.EqualTo(1));
        }

        [Test]
        public void GivenValidData_WhenThePostsAreRetrieved_AndTheDatabaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _postRepository.Setup(
                p =>
                p.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws<Exception>();
            Assert.Throws<MBlogException>(
                () =>
                _postService.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                                          It.IsAny<string>()));
        }

        [Test]
        public void GivenValidData_WhenThePostsAreRetrieved_ThenAllThePostsAreReturned()
        {
            _postRepository.Setup(
                p =>
                p.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<Post> {new Post()});
            IList<Post> posts = _postService.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(),
                                                          It.IsAny<string>(), It.IsAny<string>());
            Assert.That(posts.Count, Is.EqualTo(1));
        }

        [Test]
        public void WhenAllThePostsAreRetrieved_AndTheDatabaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _postRepository.Setup(p => p.GetPosts()).Throws<Exception>();
            Assert.Throws<MBlogException>(() => _postService.GetBlogPosts());
        }

        [Test]
        public void WhenAllThePostsAreRetrieved_ThenAllThePostsAreReturned()
        {
            _postRepository.Setup(p => p.GetPosts()).Returns(new List<Post> {new Post()});
            IList<Post> posts = _postService.GetBlogPosts();
            Assert.That(posts.Count, Is.EqualTo(1));
        }
    }
}