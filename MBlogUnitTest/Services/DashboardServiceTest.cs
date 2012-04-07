using System;
using MBlogModel;
using MBlogRepository.Interfaces;
using MBlogService;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Services
{
    [TestFixture]
    public class DashboardServiceTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            blogRepository = new Mock<IBlogRepository>();
            postRepository = new Mock<IPostRepository>();
        }

        #endregion

        private Mock<IBlogRepository> blogRepository;
        private Mock<IPostRepository> postRepository;

        [Test]
        public void
            GivenAValidPost_AndAValidBlogId_AndAnUnavailableDatabase_WhenICreateAPost_ThenAnMBlogExceptionIsThhrown()
        {
            postRepository.Setup(p => p.Create(It.IsAny<Post>())).Throws<Exception>();
            var dashboardService = new DashboardService(postRepository.Object, blogRepository.Object);
            var post = new Post();
            Assert.Throws<MBlogException>(() => dashboardService.CreatePost(post, 1));
        }

        [Test]
        public void GivenAValidPost_AndAValidBlogId_WhenICreateAPost_ThenThePostIsCreated()
        {
            var dashboardService = new DashboardService(postRepository.Object, blogRepository.Object);
            var post = new Post();
            dashboardService.CreatePost(post, 1);
            blogRepository.Verify(b => b.UpdateBlogStatistics(1), Times.Once());
            postRepository.Verify(p => p.Create(post), Times.Once());
        }

        [Test]
        public void GivenValidData_AndAnUnavailableDatabase_WhenICreateAPost_ThenAnMBlogExceptionIsThhrown()
        {
            postRepository.Setup(p => p.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Throws
                <Exception>();
            var dashboardService = new DashboardService(postRepository.Object, blogRepository.Object);

            Assert.Throws<MBlogException>(
                () => dashboardService.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
        }

        [Test]
        public void GivenValidData_WhenICreateAPost_ThenThePostIsCreated()
        {
            var dashboardService = new DashboardService(postRepository.Object, blogRepository.Object);

            dashboardService.Update(1, "title", "entry", 1);
            blogRepository.Verify(b => b.UpdateBlogStatistics(1), Times.Once());
            postRepository.Verify(p => p.Update(1, "title", "entry"));
        }
    }
}