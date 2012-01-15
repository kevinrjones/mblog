﻿using System;
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
using Newtonsoft.Json;

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
        public void GivenAMediaController_WhenMediaIsRetrievedSuccesfully_ThenAFileResultIsReturned()
        {
            _mediaDomain.Setup(m => m.GetMedia(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).
                Returns(new Media{Data = new byte[]{}, MimeType = "mimetype"});
            var controller = new MediaController(_mediaDomain.Object, null);
            FileResult result = controller.Show(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()) as FileResult;
            
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAMediaController_WhenMediaCannotBeRetrieved_ThenAHttpNotFoundResultIsReturned()
        {
            _mediaDomain.Setup(m => m.GetMedia(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Throws<MBlogMediaNotFoundException>();
            var controller = new MediaController(_mediaDomain.Object, null);
            HttpNotFoundResult result = controller.Show(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()) as HttpNotFoundResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAnInvalidFile_ThenAFalseSuccessCodeIsSet()
        {
            var controller = new MediaController(_mediaDomain.Object, null);
            JsonResult json = controller.Create(new NewMediaViewModel {Title = "title", Caption = "caption", Description = "description", Alternate = "alternate", Alignment = (int)Media.ValidAllignments.Left, Size = (int)Media.ValidSizes.Medium, File = null});
            MediaCreateJsonResponse response = (MediaCreateJsonResponse) json.Data;
            Assert.That(response.success, Is.False);
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAnInvalidFileWithNoBytes_ThenAFalseSuccessCodeIsSet()
        {

            Mock<HttpPostedFileBase> fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(0);

            MediaController controller = new MediaController(_mediaDomain.Object, null);
            JsonResult json = controller.Create(new NewMediaViewModel { Title = "title", Caption = "caption", Description = "description", Alternate = "alternate", Alignment = (int)Media.ValidAllignments.Left, Size = (int)Media.ValidSizes.Medium, File = fileBase.Object });
            MediaCreateJsonResponse response = (MediaCreateJsonResponse)json.Data;
            Assert.That(response.success, Is.False);
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

            var controller = new MediaController(_mediaDomain.Object, null);

            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = userId };

            controller.Create(new NewMediaViewModel { Title = "title", Caption = "caption", Description = "description", Alternate = "alternate", Alignment = (int)Media.ValidAllignments.Left, Size = (int)Media.ValidSizes.Medium, File = fileBase.Object });

            _mediaDomain.Verify(i => i.WriteMedia(fileName, userId, "contentType", It.IsAny<Stream>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAValidFile_ThenASuccessCodeIsSet()
        {
            byte[] fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            Mock<HttpPostedFileBase> fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));

            MediaController controller = new MediaController(_mediaDomain.Object, null);

            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };


            var result = controller.Create(new NewMediaViewModel { Title = "title", Caption = "caption", Description = "description", Alternate = "alternate", Alignment = (int)Media.ValidAllignments.Left, Size = (int)Media.ValidSizes.Medium, File = fileBase.Object });

            Assert.That(result, Is.TypeOf<JsonResult>());
            MediaCreateJsonResponse response = (MediaCreateJsonResponse)result.Data;
            Assert.That(response.success, Is.True);
        }

        [Test]
        public void GivenAMediaController_WhenNewIsCalled_ThenTheNewViewIsReturned()
        {
            MockHttpContext.SetupProperty(h => h.User);

            MediaController controller = new MediaController(_mediaDomain.Object, null);

            var result = controller.New(new NewMediaViewModel { Nickname = "nickname"});
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void WhenUpdateIsCalled_AndTheModelStateIsInvalid_ThenTheNewViewIsReturned()
        {
            MediaController controller = new MediaController(_mediaDomain.Object, null);
            NewMediaViewModel model = new NewMediaViewModel();
            controller.ViewData.ModelState.AddModelError("Key", "ErrorMessage");
            ViewResult result = (ViewResult)controller.Update(model);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("New").IgnoreCase);
        }

        [Test]
        public void WhenUpdateIsCalled_AndTheModelStateIsValid_ThenTheFileIsWrittenToTheDatabase()
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

            NewMediaViewModel model = new NewMediaViewModel
            {
                Title = "title",
                Caption = "caption",
                Description = "description",
                Alternate = "alternate",
                Alignment = (int)Media.ValidAllignments.Left,
                Size = (int)Media.ValidSizes.Large,
                File = fileBase.Object
            };
            controller.Update(model);

            _mediaDomain.Verify(i => i.WriteMedia(fileName, "title", "caption", "description", "alternate", userId, "contentType", (int)Media.ValidAllignments.Left, (int)Media.ValidSizes.Large, It.IsAny<Stream>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void GivenAMediaController_WhenIUploadAValidFile_ThenAValidUrlIsReturned()
        {
            byte[] fileBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            string fileName = "foo.jpg";
            Mock<HttpPostedFileBase> fileBase = new Mock<HttpPostedFileBase>();
            fileBase.Setup(f => f.ContentLength).Returns(fileBytes.Length);
            fileBase.Setup(s => s.InputStream).Returns(new MemoryStream(fileBytes));
            fileBase.Setup(s => s.FileName).Returns(fileName);

            MediaController controller = new MediaController(_mediaDomain.Object, null);
            DateTime now = DateTime.Now;
            _mediaDomain.Setup(
                m =>
                m.WriteMedia(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Stream>(),
                             It.IsAny<int>())).Returns(string.Format("{0}/{1}/{2}/{3}", now.Year, now.Month, now.Day,
                                                                     fileName));
            SetControllerContext(controller);
            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel { IsLoggedIn = true, Id = 1 };


            var result = controller.Create(new NewMediaViewModel { Title = "title", Caption = "caption", Description = "description", Alternate = "alternate", Alignment = (int)Media.ValidAllignments.Left, Size = (int)Media.ValidSizes.Medium, File = fileBase.Object });
            Assert.That(result, Is.TypeOf<JsonResult>());
            MediaCreateJsonResponse response = (MediaCreateJsonResponse)result.Data;
            Assert.That(response.url, Is.EqualTo(string.Format("{0}/{1}/{2}/{3}", now.Year, now.Month, now.Day, fileName)));
        }

    }
}