using System.IO;
using System.Web;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    class MediaControllerTest : BaseControllerTests
    {
        private Mock<IMediaRepository> _imageRepository;

        [SetUp]
        public void Setup()
        {
            _imageRepository = new Mock<IMediaRepository>();
        }

        [Test]
        public void GivenAnImageController_WhenIUploadAnInvalidFile_ThenAnExceptioIsThrown()
        {
            MediaController controller = new MediaController(_imageRepository.Object, null, null, null);
            Assert.Throws<MBlogException>(()=>controller.Create("title", "caption", "description", "alternate", (int) Media.ValidAllignments.Left, (int) Media.ValidSizes.Medium, null));
        }

        [Test]
        public void GivenAnImageController_WhenIUploadAnInvalidFileWithNoBytes_ThenAnExceptioIsThrown()
        {
            
            Mock<HttpPostedFileBase> fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(0);

            MediaController controller = new MediaController(_imageRepository.Object, null, null, null);
            Assert.Throws<MBlogException>(() => controller.Create("title", "caption", "description", "alternate", (int) Media.ValidAllignments.Right, (int) Media.ValidSizes.Thumbnail, fileBase.Object));
        }

        [Test]
        public void GivenAnImageController_WhenIUploadAValidFile_ThenTheFileIsWrittenToTheController()
        {
            byte[] fileBytes = new byte[]{1,2,3,4,5,6,7,8,9,0};
            
            int userId = 1001;
            var fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));
            fileBase.Setup(s => s.ContentType).Returns("contentType");
            const string fileName = "fileName";
            fileBase.Setup(s => s.FileName).Returns(fileName);

            Media mediaToWrite = new Media { FileName = fileName, Title = "title", Caption = "caption", 
                Description = "description", Alternate = "alternate", UserId = userId,
                                             MimeType = "contentType",
                                             Alignment = (int) Media.ValidAllignments.None,
                                             Size = (int) Media.ValidSizes.Large,
                                             ImageData = fileBytes
            };
            _imageRepository.Setup(i => i.WriteMedia(mediaToWrite));

            var controller = new MediaController(_imageRepository.Object, null, null, null);
            
            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = userId };

            controller.Create("title", "caption", "description", "alternate", (int) Media.ValidAllignments.Left, (int) Media.ValidSizes.Large, fileBase.Object);

            _imageRepository.Verify(i => i.WriteMedia(It.IsAny<Media>()), Times.Once());
        }

        [Test]
        public void GivenAnImageController_WhenIUploadAValidFile_ThenTheControllerRedirectsToTheCorrectPage()
        {
            byte[] fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            Mock<HttpPostedFileBase> fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));

            MediaController controller = new MediaController(_imageRepository.Object, null, null, null);

            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };


            var result = controller.Create("title", "caption", "description", "alternate", (int) Media.ValidAllignments.Right, (int) Media.ValidSizes.Medium, fileBase.Object);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
        }

        [Test]
        public void GivenAnImageController_WhenNewIsCalled_ThenTheNewViewIsReturned()
        {
            MockHttpContext.SetupProperty(h => h.User);

            MediaController controller = new MediaController(_imageRepository.Object, null, null, null);

            var result = controller.New("nickname", 1);
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

    }
}
