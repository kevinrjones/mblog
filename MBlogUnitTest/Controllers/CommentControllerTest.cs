using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models.Comment;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    public class CommentControllerTest : BaseControllerTests
    {
        private Mock<IPostRepository> _postUserRepository;
        private CommentController _controller;
        private string _expectedRefererUrl = "value";

        [SetUp]
        public void SetUp()
        {
            _postUserRepository = new Mock<IPostRepository>();
            _controller = new CommentController(_postUserRepository.Object, null, null);
            var headers = new FormCollection();
            headers.Add("Referer", _expectedRefererUrl);
            MockRequest.Setup(r => r.Headers).Returns(headers);

            SetControllerContext(_controller);
        }

        [Test]
        public void GivenACommentController_WhenCreateIsCalled_ThenTheCommentIsAdded()
        {
            string name = "Name";
            string comment = "Comment";

            _postUserRepository.Setup(p => p.AddComment(1, name, comment)).Verifiable();
            _controller.Create(new AddCommentViewModel(1, true){Name = name, Comment = comment});
            _postUserRepository.Verify(p => p.AddComment(1, name, comment));
        }

        [Test]
        public void GivenACommentController_WhenCreateIsCalled_AndTheCommentIsInvalid_ThenTheCommentStateIsAddedToTempData()
        {
            string name = "Name";
            string comment = "Comment";

            _postUserRepository.Setup(p => p.AddComment(1, name, comment)).Verifiable();
            _controller.ModelState.AddModelError("Name", "Name error");
            _controller.Create(new AddCommentViewModel(1, true) { Name = name, Comment = comment });
            Assert.That(_controller.TempData, Is.Not.Null);
            Assert.That(_controller.TempData["comment"], Is.Not.Null);
        }

        [Test]
        public void GivenACommentController_WhenCreateIsCalled_ThenTheBrowserIsRedirectedToTheReferer()
        {
            string name = "Name";
            string comment = "Comment";

            _postUserRepository.Setup(p => p.AddComment(1, name, comment)).Verifiable();
            RedirectResult result = (RedirectResult) _controller.Create(new AddCommentViewModel(1, true) { Name = name, Comment = comment });
            Assert.That(result.Url, Is.EqualTo(_expectedRefererUrl));
                
        }
    }
}
