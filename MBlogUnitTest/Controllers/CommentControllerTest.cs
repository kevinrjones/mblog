using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models.Comment;
using MBlogDomainInterfaces;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    public class CommentControllerTest : BaseControllerTests
    {
        private Mock<IPostDomain> _postDomain;
        private CommentController _controller;
        private string _expectedRefererUrl = "value";

        [SetUp]
        public void SetUp()
        {
            _postDomain = new Mock<IPostDomain>();
            _controller = new CommentController(_postDomain.Object, null);
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

            _controller.Create(new AddCommentViewModel(1, true){Name = name, Comment = comment});
            _postDomain.Verify(p => p.AddComment(1, name, comment));
        }

        [Test]
        public void GivenACommentController_WhenCreateIsCalled_AndTheCommentIsInvalid_ThenTheCommentStateIsAddedToTempData()
        {
            string name = "Name";
            string comment = "Comment";

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

            RedirectResult result = (RedirectResult) _controller.Create(new AddCommentViewModel(1, true) { Name = name, Comment = comment });
            Assert.That(result.Url, Is.EqualTo(_expectedRefererUrl));
            _postDomain.Verify(p => p.AddComment(1, name, comment));                
        }
    }
}
