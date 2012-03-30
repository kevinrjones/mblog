using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlogModel;
using MBlogRepository.Interfaces;
using MBlogService;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Domain
{
    [TestFixture]
    public class DashboardDomainTest
    {
        Mock<IBlogRepository> blogRepository;
        Mock<IPostRepository> postRepository;
        
        [SetUp]
        public void Setup()
        {
            blogRepository = new Mock<IBlogRepository>();
            postRepository = new Mock<IPostRepository>();
        }

        [Test]
        public void GivenAValidPost_AndAValidBlogId_WhenICreateAPost_ThenThePostIsCreated()
        {
            DashboardService dashboardService = new DashboardService(postRepository.Object, blogRepository.Object);
            Post post = new Post();
            dashboardService.CreatePost(post, 1);
            blogRepository.Verify(b => b.ChangeBlogLastupdateDate(1), Times.Once());
            postRepository.Verify(p => p.Create(post), Times.Once());
        }

        [Test]
        public void GivenAValidPost_AndAValidBlogId_AndAnUnavailableDatabase_WhenICreateAPost_ThenAnMBlogExceptionIsThhrown()
        {
            postRepository.Setup(p => p.Create(It.IsAny<Post>())).Throws<Exception>();
            DashboardService dashboardService = new DashboardService(postRepository.Object, blogRepository.Object);
            Post post = new Post();
            Assert.Throws<MBlogException>(() => dashboardService.CreatePost(post, 1));
        }

        [Test]
        public void GivenValidData_WhenICreateAPost_ThenThePostIsCreated()
        {
            DashboardService dashboardService = new DashboardService(postRepository.Object, blogRepository.Object);

            dashboardService.Update(1, "title", "entry", 1);
            blogRepository.Verify(b => b.ChangeBlogLastupdateDate(1), Times.Once());
            postRepository.Verify(p=>p.Update(1, "title", "entry"));
        }

        [Test]
        public void GivenValidData_AndAnUnavailableDatabase_WhenICreateAPost_ThenAnMBlogExceptionIsThhrown()
        {
            postRepository.Setup(p => p.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            DashboardService dashboardService = new DashboardService(postRepository.Object, blogRepository.Object);

            Assert.Throws<MBlogException>(() => dashboardService.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
        }
    }
}
