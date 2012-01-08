using System.IO;
using System.Web;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models.Media;
using MBlog.Models.User;
using MBlogDomainInterfaces;
using MBlogModel;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    class MediaControllerTest : BaseControllerTests
    {
        private Mock<IMediaDomain> _mediaDomain;

        [SetUp]
        public void Setup()
        {
            _mediaDomain = new Mock<IMediaDomain>();
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAnInvalidFile_ThenAnExceptioIsThrown()
        {
            MediaController controller = new MediaController(_mediaDomain.Object, null);
            Assert.Throws<MBlogException>(()=>controller.Create("title", "caption", "description", "alternate", (int) Media.ValidAllignments.Left, (int) Media.ValidSizes.Medium, null));
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAnInvalidFileWithNoBytes_ThenAnExceptioIsThrown()
        {
            
            Mock<HttpPostedFileBase> fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(0);

            MediaController controller = new MediaController(_mediaDomain.Object, null);
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

            var controller = new MediaController(_mediaDomain.Object, null);
            
            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = userId };

            controller.Create("title", "caption", "description", "alternate", (int) Media.ValidAllignments.Left, (int) Media.ValidSizes.Large, fileBase.Object);

            _mediaDomain.Verify(i => i.WriteMedia(fileName, "title", "caption", "description", "alternate", userId, "contentType", (int) Media.ValidAllignments.Left, (int) Media.ValidSizes.Large, It.IsAny<Stream>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAValidFile_ThenTheControllerRedirectsToTheCorrectPage()
        {
            byte[] fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            Mock<HttpPostedFileBase> fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));

            MediaController controller = new MediaController(_mediaDomain.Object, null);

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

            MediaController controller = new MediaController(_mediaDomain.Object, null);

            var result = controller.New(new NewMediaViewModel{Nickname = "nickname", BlogId = 1});
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void WhenUploadIsCalled_AndTheModelStateIsInvalid_ThenTheNewViewIsReturned()
        {
            MediaController controller = new MediaController(_mediaDomain.Object, null);
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

            var controller = new MediaController(_mediaDomain.Object, null);

            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = userId };

            NewMediaViewModel model = new NewMediaViewModel { Title = "title", Caption = "caption", Description = "description", Alternate = "alternate", 
                Alignment = (int)Media.ValidAllignments.Left, Size = (int)Media.ValidSizes.Large, File = fileBase.Object };
            controller.Upload(model);

            _mediaDomain.Verify(i => i.WriteMedia(fileName, userId, "contentType", It.IsAny<Stream>(), It.IsAny<int>()), Times.Once());
        }
    }
}
