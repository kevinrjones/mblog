using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MBlog.Controllers;
using MBlog.Models.Media;
using MBlog.Models.User;
using MBlogModel;
using MBlogServiceInterfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    internal class MediaControllerTest : BaseControllerTests
    {

        [SetUp]
        public void Setup()
        {
            _mediaService = new Mock<IMediaService>();
        }


        private Mock<IMediaService> _mediaService;

        [Test]
        public void GivenAMediaController_WhenIUploadAValidFileUsingHttpContext_ThenTheFileIsWrittenToTheDatabase()
        {
            //Assert.Fail("Write the test");
            var fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            int userId = 1001;
            var fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));
            fileBase.Setup(s => s.ContentType).Returns("contentType");
            const string fileName = "fileName";
            fileBase.Setup(s => s.FileName).Returns(fileName);

            var controller = new MediaController(_mediaService.Object, null);

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

            _mediaService.Verify(
                i => i.WriteMedia(fileName, userId, "contentType", It.IsAny<Stream>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAValidFile_ThenASuccessCodeIsSet()
        {
            var fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            var fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));
            string fileName = "foo.jpg";
            DateTime now = DateTime.Now;
            Media media = new Media { Year = now.Year, Month = now.Month, Day = now.Day, LinkKey = fileName };
            _mediaService.Setup(m => m.WriteMedia(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Stream>(),
                 It.IsAny<int>())).Returns(media);

            var controller = new MediaController(_mediaService.Object, null);
            controller.Url = new UrlHelper(new RequestContext(MockHttpContext.Object, new RouteData()), Routes);

            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };


            JsonResult result =
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

            Assert.That(result, Is.TypeOf<JsonResult>());
            var response = (MediaCreateJsonResponse)result.Data;
            Assert.That(response.success, Is.True);
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAValidFile_ThenAValidUrlIsReturned()
        {
            var fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            string fileName = "foo.jpg";
            var fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));
            fileBase.Setup(s => s.FileName).Returns(fileName);

            var controller = new MediaController(_mediaService.Object, null);
            controller.Url = new UrlHelper(new RequestContext(MockHttpContext.Object, new RouteData()), Routes);
            DateTime now = DateTime.Now;
            Media media = new Media { Year = now.Year, Month = now.Month, Day = now.Day, LinkKey = fileName };
            _mediaService.Setup(
                m => m.WriteMedia(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Stream>(),
                             It.IsAny<int>())).Returns(media);
            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };


            JsonResult result =
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
            Assert.That(result, Is.TypeOf<JsonResult>());
            var response = (MediaCreateJsonResponse)result.Data;
            Assert.That(response.url,
                        Is.EqualTo(string.Format("{0}/{1}/{2}/{3}", now.Year, now.Month, now.Day, fileName)));
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAValidFile_ThenTheFileIsWrittenToTheDatabase()
        {
            var fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            int userId = 1001;
            var fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));
            fileBase.Setup(s => s.ContentType).Returns("contentType");
            const string fileName = "fileName";
            fileBase.Setup(s => s.FileName).Returns(fileName);

            var controller = new MediaController(_mediaService.Object, null);

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

            _mediaService.Verify(
                i => i.WriteMedia(fileName, userId, "contentType", It.IsAny<Stream>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAnInvalidFileWithNoBytes_ThenAFalseSuccessCodeIsSet()
        {
            var fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(0);

            var controller = new MediaController(_mediaService.Object, null);
            JsonResult json =
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
            var response = (MediaCreateJsonResponse)json.Data;
            Assert.That(response.success, Is.False);
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAnInvalidFile_ThenAFalseSuccessCodeIsSet()
        {
            var controller = new MediaController(_mediaService.Object, null);
            JsonResult json =
                controller.Create(new NewMediaViewModel
                                      {
                                          Title = "title",
                                          Caption = "caption",
                                          Description = "description",
                                          Alternate = "alternate",
                                          Alignment = (int)Media.ValidAllignments.Left,
                                          Size = (int)Media.ValidSizes.Medium,
                                          File = null
                                      });
            var response = (MediaCreateJsonResponse)json.Data;
            Assert.That(response.success, Is.False);
        }

        [Test]
        public void GivenAMediaController_WhenMediaCannotBeRetrieved_ThenAHttpNotFoundResultIsReturned()
        {
            _mediaService.Setup(m => m.GetMedia(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).
                Throws<MBlogMediaNotFoundException>();
            var controller = new MediaController(_mediaService.Object, null);
            var result =
                controller.Show(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()) as
                HttpNotFoundResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAMediaController_WhenMediaIsRetrievedSuccesfully_ThenAFileResultIsReturned()
        {
            _mediaService.Setup(m => m.GetMedia(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).
                Returns(new Media { Data = new byte[] { }, MimeType = "mimetype" });
            var controller = new MediaController(_mediaService.Object, null);
            var result =
                controller.Show(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()) as FileResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAMediaController_WhenNewIsCalled_ThenTheNewViewIsReturned()
        {
            MockHttpContext.SetupProperty(h => h.User);

            var controller = new MediaController(_mediaService.Object, null);

            ActionResult result = controller.New(new NewMediaViewModel { Nickname = "nickname" });
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void WhenUpdateIsCalled_AndTheModelStateIsInvalid_ThenTheCorrectViewIsReturned()
        {
            var controller = new MediaController(_mediaService.Object, null);
            var model = new UpdateMediaViewModel();
            controller.ViewData.ModelState.AddModelError("Key", "ErrorMessage");
            var result = (JsonResult)controller.Update(model);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void WhenUpdateIsCalled_AndTheModelStateIsValid_ThenTheFileIsWrittenToTheDatabase()
        {
            var fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            const int userId = 1001;
            var fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));
            fileBase.Setup(s => s.ContentType).Returns("contentType");
            const string fileName = "fileName";
            fileBase.Setup(s => s.FileName).Returns(fileName);
            _mediaService.Setup(m => m.UpdateMediaDetails(1, "title", "caption", "description", "alternate", 1001)).
                Returns(new Media());

            var controller = new MediaController(_mediaService.Object, null);
            controller.Url = new UrlHelper(new RequestContext(MockHttpContext.Object, new RouteData()), Routes);
            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = userId };

            var model = new UpdateMediaViewModel
            {
                Id = 1,
                Title = "title",
                Caption = "caption",
                Description = "description",
                Alternate = "alternate",
            };

            controller.Update(model);
            
            _mediaService.Verify(i => i.UpdateMediaDetails(1, "title", "caption", "description", "alternate", 1001), Times.Once());
        }
    }
}