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
    class ImageControllerTest : BaseControllerTests
    {
        private Mock<IImageRepository> _imageRepository;

        [SetUp]
        public void Setup()
        {
            _imageRepository = new Mock<IImageRepository>();
        }

        [Test]
        public void GivenAnImageController_WhenIUploadAnInvalidFile_ThenAnExceptioIsThrown()
        {
            ImageController controller = new ImageController(_imageRepository.Object, null, null, null);
            Assert.Throws<MBlogException>(()=>controller.Create("nickname", 1, "title", "caption", "description", "alternate", "alignment", (int) Image.ValidSizes.Medium, null));
        }

        [Test]
        public void GivenAnImageController_WhenIUploadAnInvalidFileWithNoBytes_ThenAnExceptioIsThrown()
        {
            
            Mock<HttpPostedFileBase> fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(0);

            ImageController controller = new ImageController(_imageRepository.Object, null, null, null);
            Assert.Throws<MBlogException>(() => controller.Create("nickname", 1, "title", "caption", "description", "alternate", "alignment", (int) Image.ValidSizes.Thumbnail, fileBase.Object));
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

            Image imageToWrite = new Image { FileName = fileName, Title = "title", Caption = "caption", 
                Description = "description", Alternate = "alternate", UserId = userId,
                                             MimeType = "contentType",
                                             Alignment = "alignment",
                                             Size = (int) Image.ValidSizes.Large,
                                             ImageData = fileBytes
            };
            _imageRepository.Setup(i => i.WriteImage(imageToWrite));

            var controller = new ImageController(_imageRepository.Object, null, null, null);
            
            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = userId };

            controller.Create("nickname", 1, "title", "caption", "description", "alternate", "alignment", (int) Image.ValidSizes.Large, fileBase.Object);

            _imageRepository.Verify(i => i.WriteImage(It.IsAny<Image>()), Times.Once());
        }

        [Test]
        public void GivenAnImageController_WhenIUploadAValidFile_ThenTheControllerRedirectsToTheCorrectPage()
        {
            byte[] fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            Mock<HttpPostedFileBase> fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));

            ImageController controller = new ImageController(_imageRepository.Object, null, null, null);

            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };


            var result = controller.Create("nickname", 1, "title", "caption", "description", "alternate", "alignment", (int) Image.ValidSizes.Medium, fileBase.Object);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
        }

        [Test]
        public void GivenAnImageController_WhenNewIsCalled_ThenTheNewViewIsReturned()
        {
            MockHttpContext.SetupProperty(h => h.User);

            ImageController controller = new ImageController(_imageRepository.Object, null, null, null);

            var result = controller.New("nickname", 1);
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

    }
}
