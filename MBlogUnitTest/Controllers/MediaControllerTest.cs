using System.Collections.Specialized;
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
        public void GivenAMediaController_WhenIUploadAnInvalidFile_AndThereIsNoValuesInTheHeaders_ThenAFailureResultIsReturned()
        {
            MediaController controller = new MediaController(_mediaRepository.Object, null, null, null);
            JsonResult result = controller.Create(new NewMediaViewModel
                                                                    {
                                                                        Title = "title",
                                                                        Caption = "caption",
                                                                        Description = "description",
                                                                        Alternate = "alternate",
                                                                        Alignment = (int)Media.ValidAllignments.Left,
                                                                        Size = (int)Media.ValidSizes.Medium,
                                                                        File = null
                                                                    });

            Assert.That(result, Is.Not.Null);
            dynamic data = result.Data;
            Assert.That(data.success, Is.False);
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAnInvalidFileWithNoBytes_ThenAnExceptionIsThrown()
        {

            Mock<HttpPostedFileBase> fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(0);

            MediaController controller = new MediaController(_mediaRepository.Object, null, null, null);
            JsonResult result = controller.Create(new NewMediaViewModel
                                                            {
                                                                Title = "title",
                                                                Caption = "caption",
                                                                Description = "description",
                                                                Alternate = "alternate",
                                                                Alignment = (int)Media.ValidAllignments.Left,
                                                                Size = (int)Media.ValidSizes.Medium,
                                                                File = fileBase.Object
                                                            });

            Assert.That(result, Is.Not.Null);
            dynamic data = result.Data;
            Assert.That(data.success, Is.False);
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAValidFile_ThenTheFileIsWrittenToTheDatabase()
        {
            byte[] fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            int userId = 1001;
            var fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));
            fileBase.Setup(s => s.ContentType).Returns("contentType");
            const string fileName = "fileName";
            fileBase.Setup(s => s.FileName).Returns(fileName);

            Media mediaToWrite = new Media
            {
                FileName = fileName,
                Title = "title",
                Caption = "caption",
                Description = "description",
                Alternate = "alternate",
                UserId = userId,
                MimeType = "contentType",
                Alignment = (int)Media.ValidAllignments.None,
                Size = (int)Media.ValidSizes.Large,
                Data = fileBytes
            };
            _mediaRepository.Setup(i => i.WriteMedia(mediaToWrite));

            var controller = new MediaController(_mediaRepository.Object, null, null, null);

            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = userId };

            controller.Create(new NewMediaViewModel
                                                    {
                                                        Title = "title",
                                                        Caption = "caption",
                                                        Description = "description",
                                                        Alternate = "alternate",
                                                        Alignment = (int)Media.ValidAllignments.Left,
                                                        Size = (int)Media.ValidSizes.Medium,
                                                        File = fileBase.Object
                                                    });


            _mediaRepository.Verify(i => i.WriteMedia(It.IsAny<Media>()), Times.Once());
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAValidFile_ThenTheControllerRedirectsToTheCorrectPage()
        {
            byte[] fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            Mock<HttpPostedFileBase> fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.FileName).Returns("file.txt");
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));

            MediaController controller = new MediaController(_mediaRepository.Object, null, null, null);

            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };


            var result = controller.Create(new NewMediaViewModel
                                                    {
                                                        Title = "title.txt",
                                                        Caption = "caption",
                                                        Description = "description",
                                                        Alternate = "alternate",
                                                        Alignment = (int)Media.ValidAllignments.Left,
                                                        Size = (int)Media.ValidSizes.Medium,
                                                        File = fileBase.Object
                                                    });


            Assert.That(result, Is.TypeOf<JsonResult>());
            dynamic data = result.Data;
            Assert.That(data.success, Is.True);
        }

        [Test]
        public void GivenAMediaController_WhenNewIsCalled_ThenTheNewViewIsReturned()
        {
            MockHttpContext.SetupProperty(h => h.User);

            MediaController controller = new MediaController(_mediaRepository.Object, null, null, null);

            var result = controller.New(new NewMediaViewModel { Nickname = "nickname", BlogId = 1 });
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void WhenUpdateIsCalled_AndAFileIsPassed_ThenTheFileIsWrittenToTheDatabase()
        {
            byte[] fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            const int userId = 0;
            var fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));
            fileBase.Setup(s => s.ContentType).Returns("contentType");
            const string fileName = "fileName.txt";
            fileBase.Setup(s => s.FileName).Returns(fileName);

            Media mediaToWrite = new Media
            {
                FileName = fileName,
                Title = "fileName",
                Caption = "",
                Description = "",
                Alternate = "",
                UserId = userId,
                MimeType = "contentType",
                Alignment = 0,
                Size = 0,
                Data = fileBytes
            };
            _mediaRepository.Setup(i => i.WriteMedia(mediaToWrite));

            var controller = new MediaController(_mediaRepository.Object, null, null, null);

            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = userId };

            NewMediaViewModel model = new NewMediaViewModel
            {
                Title = "title",
                Caption = "caption",
                Description = "description",
                Alternate = "alternate",
                Alignment = (int)Media.ValidAllignments.Left,
                Size = (int)Media.ValidSizes.Large,
                QqFile = fileName,
                File = fileBase.Object
            };
            controller.Create(model);

            _mediaRepository.Verify(i => i.WriteMedia(mediaToWrite), Times.Once());
        }

        [Test]
        public void WhenUpdateIsCalled_AndThePostBodyHasTheData_ThenTheFileIsWrittenToTheDatabase()
        {
            byte[] fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            const int userId = 0;
            //NameValueCollection headers = new NameValueCollection();
            //headers.Add("ContentLength", fileBytes.Length.ToString());
            MockRequest.Setup(r => r.ContentLength).Returns(fileBytes.Length);
            MockRequest.Setup(r => r.InputStream).Returns(new MemoryStream(fileBytes));
            const string fileName = "fileName.jpg";

            Media mediaToWrite = new Media
            {               
                FileName = fileName,
                Title = "fileName",
                Caption = "",
                Description = "",
                Alternate = "",
                UserId = userId,
                MimeType = "image/jpeg",
                Alignment = 0,
                Size = 0,
                Data = fileBytes
            };
            _mediaRepository.Setup(i => i.WriteMedia(mediaToWrite));

            var controller = new MediaController(_mediaRepository.Object, null, null, null);

            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = userId };

            NewMediaViewModel model = new NewMediaViewModel
            {
                Title = "title",
                Caption = "caption",
                Description = "description",
                Alternate = "alternate",
                Alignment = (int)Media.ValidAllignments.Left,
                Size = (int)Media.ValidSizes.Large,
                File = null,
                QqFile = fileName
            };
            controller.Create(model);

            _mediaRepository.Verify(i => i.WriteMedia(mediaToWrite), Times.Once());
        }
    }
}
