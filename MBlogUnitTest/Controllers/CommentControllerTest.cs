using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models.Comment;
using MBlogServiceInterfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    public class CommentControllerTest : BaseControllerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _postDomain = new Mock<IPostService>();
            _controller = new CommentController(_postDomain.Object, null);
            var headers = new FormCollection {{"Referer", ExpectedRefererUrl}};
            MockRequest.Setup(r => r.Headers).Returns(headers);

            SetControllerContext(_controller);
        }

        #endregion

        private Mock<IPostService> _postDomain;
        private CommentController _controller;
        private const string ExpectedRefererUrl = "value";
        private const string Name = "Name";
        private const string Comment = "Comment";

        [Test]
        public void
            GivenACommentController_WhenCreateIsCalled_AndTheCommentIsInvalid_ThenTheCommentStateIsAddedToTempData()
        {
            _controller.ModelState.AddModelError("Name", "Name error");
            _controller.Create(new AddCommentViewModel(1, true) {Name = Name, Comment = Comment});
            Assert.That(_controller.TempData, Is.Not.Null);
            Assert.That(_controller.TempData["comment"], Is.Not.Null);
        }

        [Test]
        public void GivenACommentController_WhenCreateIsCalled_ThenTheBrowserIsRedirectedToTheReferer()
        {
            var result =
                (RedirectResult) _controller.Create(new AddCommentViewModel(1, true) {Name = Name, Comment = Comment});
            Assert.That(result.Url, Is.EqualTo(ExpectedRefererUrl));
            _postDomain.Verify(p => p.AddComment(1, Name, Comment));
        }

        [Test]
        public void GivenACommentController_WhenCreateIsCalled_ThenTheCommentIsAdded()
        {
            _controller.Create(new AddCommentViewModel(1, true) {Name = Name, Comment = Comment});
            _postDomain.Verify(p => p.AddComment(1, Name, Comment));
        }
    }
}