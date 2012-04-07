using System;
using MBlogModel;
using MBlogRepository.Interfaces;
using MBlogService;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Services
{
    [TestFixture]
    public class BlogServiceTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _blogRepository = new Mock<IBlogRepository>();
        }

        #endregion

        private Mock<IBlogRepository> _blogRepository;

        [Test]
        public void GivenANickname_WhenABlogIsRequested_AndTheDataBaseIsUnavailable_ThenAnMBlogExceptionIsThrown()
        {
            _blogRepository.Setup(b => b.GetBlog(It.IsAny<string>())).Throws<Exception>();
            var blogDomain = new BlogService(_blogRepository.Object);
            Assert.Throws<MBlogException>(() => blogDomain.GetBlog(It.IsAny<string>()));
        }

        [Test]
        public void GivenAValidNickname_WhenABlogIsRequested_ThenABlogIsReturned()
        {
            _blogRepository.Setup(b => b.GetBlog(It.IsAny<string>())).Returns(new Blog());
            var blogDomain = new BlogService(_blogRepository.Object);
            Blog blog = blogDomain.GetBlog(It.IsAny<string>());
            Assert.That(blog, Is.Not.Null);
        }

        [Test]
        public void GivenAValidViewModel_WhenABlogIsUpdated_AndTheDataBaseIsUnavailable_ThenAnMBlogExceptionIsThrown()
        {
            _blogRepository.Setup(b => b.GetBlog(It.IsAny<string>())).Throws<Exception>();
            var blogDomain = new BlogService(_blogRepository.Object);
            Assert.Throws<MBlogException>(
                () =>
                blogDomain.UpdateBlog(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(),
                                      It.IsAny<string>()));
        }

        [Test]
        public void GivenAValidViewModel_WhenABlogIsUpdated_ThenABlogIsUpdatedInTheRepository()
        {
            _blogRepository.Setup(b => b.GetBlog(It.IsAny<string>())).Returns(new Blog());
            var blogDomain = new BlogService(_blogRepository.Object);
            blogDomain.UpdateBlog("nickname", It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>());
            _blogRepository.Verify(b => b.GetBlog("nickname"), Times.Once());
            _blogRepository.Verify(b => b.Update(It.IsAny<Blog>()), Times.Once());
        }

        [Test]
        public void GivenValidBlogDetails_WhenABlogIsCreated_AndTheDatabaseIsUnavailable_ThenAnMBlogExceptionIsThrown()
        {
            var blogDomain = new BlogService(_blogRepository.Object);
            _blogRepository.Setup(b => b.Create(It.IsAny<Blog>())).Throws<Exception>();
            Assert.Throws<MBlogException>(
                () =>
                blogDomain.CreateBlog(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(),
                                      It.IsAny<string>(), It.IsAny<int>()));
        }

        [Test]
        public void GivenValidBlogDetails_WhenABlogIsCreated_ThenTheDatabaseIsUpdated()
        {
            var blogDomain = new BlogService(_blogRepository.Object);
            blogDomain.CreateBlog(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(),
                                  It.IsAny<string>(), It.IsAny<int>());
            _blogRepository.Verify(b => b.Create(It.IsAny<Blog>()));
        }
    }
}