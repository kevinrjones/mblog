using System.IO;
using System.Web;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models.Media;
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
        private Mock<IMediaRepository> _mediaRepository;

        [SetUp]
        public void Setup()
        {
            _mediaRepository = new Mock<IMediaRepository>();
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAnInvalidFile_ThenAnExceptioIsThrown()
        {
            MediaController controller = new MediaController(_mediaRepository.Object, null, null, null);
            Assert.Throws<MBlogException>(()=>controller.Create("title", "caption", "description", "alternate", (int) Media.ValidAllignments.Left, (int) Media.ValidSizes.Medium, null));
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAnInvalidFileWithNoBytes_ThenAnExceptioIsThrown()
        {
            
            Mock<HttpPostedFileBase> fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(0);

            MediaController controller = new MediaController(_mediaRepository.Object, null, null, null);
            Assert.Throws<MBlogException>(() => controller.Create("title", "caption", "description", "alternate", (int) Media.ValidAllignments.Right, (int) Media.ValidSizes.Thumbnail, fileBase.Object));
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAValidFile_ThenTheFileIsWrittenToTheDatabase()
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
                                             MediumData = fileBytes
            };
            _mediaRepository.Setup(i => i.WriteMedia(mediaToWrite));

            var controller = new MediaController(_mediaRepository.Object, null, null, null);
            
            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = userId };

            controller.Create("title", "caption", "description", "alternate", (int) Media.ValidAllignments.Left, (int) Media.ValidSizes.Large, fileBase.Object);

            _mediaRepository.Verify(i => i.WriteMedia(It.IsAny<Media>()), Times.Once());
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAValidFile_ThenTheControllerRedirectsToTheCorrectPage()
        {
            byte[] fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            Mock<HttpPostedFileBase> fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));

            MediaController controller = new MediaController(_mediaRepository.Object, null, null, null);

            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };


            var result = controller.Create("title", "caption", "description", "alternate", (int) Media.ValidAllignments.Right, (int) Media.ValidSizes.Medium, fileBase.Object);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
        }

        [Test]
        public void GivenAMediaController_WhenNewIsCalled_ThenTheNewViewIsReturned()
        {
            MockHttpContext.SetupProperty(h => h.User);

            MediaController controller = new MediaController(_mediaRepository.Object, null, null, null);

            var result = controller.New(new NewMediaViewModel{Nickname = "nickname", BlogId = 1});
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void WhenUploadIsCalled_AndTheModelStateIsInvalid_ThenTheNewViewIsReturned()
        {
            MediaController controller = new MediaController(_mediaRepository.Object, null, null, null);
            NewMediaViewModel model = new NewMediaViewModel();
            controller.ViewData.ModelState.AddModelError("Key", "ErrorMessage"); 
            ViewResult result = (ViewResult)controller.Upload(model);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("New").IgnoreCase);
        }

        [Test]
        public void WhenUploadIsCalled_AndTheModelStateIsValid_ThenTheFileIsWrittenToTheDatabase()
        {
            byte[] fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            const int userId = 1001;
            var fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));
            fileBase.Setup(s => s.ContentType).Returns("contentType");
            const string fileName = "fileName";
            fileBase.Setup(s => s.FileName).Returns(fileName);

            Media mediaToWrite = new Media
            {
                FileName = fileName,
                Title = "",
                Caption = "",
                Description = "",
                Alternate = "",
                UserId = userId,
                MimeType = "contentType",
                Alignment = 0,
                Size = 0,
                MediumData = fileBytes
            };
            _mediaRepository.Setup(i => i.WriteMedia(mediaToWrite));

            var controller = new MediaController(_mediaRepository.Object, null, null, null);

            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = userId };

            NewMediaViewModel model = new NewMediaViewModel { Title = "title", Caption = "caption", Description = "description", Alternate = "alternate", 
                Alignment = (int)Media.ValidAllignments.Left, Size = (int)Media.ValidSizes.Large, File = fileBase.Object };
            controller.Upload(model);

            _mediaRepository.Verify(i => i.WriteMedia(mediaToWrite), Times.Once());
        }
    }
}
