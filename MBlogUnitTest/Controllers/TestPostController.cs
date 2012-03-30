using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models.Post;
using MBlogModel;
using MBlogRepository.Interfaces;
using MBlogServiceInterfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    public class TestPostController : BaseControllerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _postDomainMock = new Mock<IPostService>();

            var dashboardDomainMock = new Mock<IDashboardService>();
            _dashboardServiceMock = dashboardDomainMock.Object;

            _postDomainMock.Setup(r => r.GetBlogPosts(It.IsAny<string>())).Returns(posts);
            _postDomainMock.Setup(
                r =>
                r.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<Post>
                             {
                                 new Post
                                     {
                                         Id = 1,
                                         BlogPost = "empty",
                                         Title = "empty",
                                         Posted = DateTime.Today,
                                         Comments =
                                             new List<Comment>
                                                 {
                                                     new Comment {CommentText = "empty", Approved = true},
                                                     new Comment {CommentText = "empty", Approved = false}
                                                 }
                                     }
                             });
            _postDomainMock.Setup(r => r.GetBlogPost(It.IsAny<int>())).Returns(posts[0]);
        }

        #endregion

        private Mock<IPostService> _postDomainMock;
        private IDashboardService _dashboardServiceMock;
        private string _userName = "UserName";
        private int _blogId = 10;

        private readonly List<Post> posts = new List<Post>
                                                {
                                                    new Post {Title = "title1", BlogPost = ""},
                                                    new Post {Title = "title2"},
                                                    new Post {Title = "title3"}
                                                };

        [Test]
        public void GivenAPostController_WhenICallItsCreateMethod_AndTheModelIsInvalid_ThenItReturnsTheCorrectView()
        {
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);
            controller.ModelState.AddModelError("Title", "Title error");
            ActionResult result = controller.Create(new EditPostViewModel());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void GivenAPostController_WhenICallItsCreateMethod_AndTheModelIsValid_ThenItReturnsTheCorrectView()
        {
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);
            var result = controller.Create(new EditPostViewModel()) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("index").IgnoreCase);
        }

        [Test]
        public void GivenAPostController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectNumberOfPosts()
        {
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);
            var result = (ViewResult) controller.Index("");

            var model = (PostsViewModel) result.Model;

            Assert.That(model.Posts.Count(), Is.EqualTo(posts.Count));
        }

        [Test]
        public void GivenAPostController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectView()
        {
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);
            var result = (ViewResult) controller.Index("");

            var model = (PostsViewModel) result.Model;

            Assert.That(model.Posts.Count(), Is.EqualTo(posts.Count));
        }

        [Test]
        public void GivenAPostController_WhenICallItsNewMethod_ThenItReturnsTheCorrectView()
        {
            MockHttpContext.SetupProperty(h => h.User);

            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);
            var result = controller.New(_userName, _blogId) as ViewResult;

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_AndIPassAYear_ThenItReturnsTheCorrectPosts()
        {
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);
            var result =
                controller.Show(new PostLinkViewModel {Year = 2010, Month = 0, Day = 0, Link = null}) as ViewResult;
            var model = (PostsViewModel) result.Model;

            Assert.That(model.Posts.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_ThenItReturnsTheCorrectModelInTheView()
        {
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);
            var result = controller.Show(new PostLinkViewModel()) as ViewResult;

            Assert.That(result.Model, Is.AssignableTo<PostsViewModel>());
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_ThenItReturnsTheCorrectView()
        {
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);
            var result = controller.Show(new PostLinkViewModel()) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAPostController_WhenIGetItsPostsWithComments_ThenItReturnsTheApprovedComments()
        {
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);
            var result = controller.Show(new PostLinkViewModel()) as ViewResult;
            var model = (PostsViewModel) result.Model;

            Assert.That(model.Posts.Count, Is.EqualTo(1));
            Assert.That(model.Posts[0].CommentCount, Is.EqualTo(1));
        }

        [Test]
        public void GivenAPostController_WhenIIAskToShowASinglePost_ThenItReturnsTheCorrectView()
        {
            var postRepositoryMock = new Mock<IPostRepository>();

            postRepositoryMock.Setup(
                r =>
                r.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<Post> {new Post {Title = "title"}});
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);
            var result =
                controller.Show(new PostLinkViewModel {Year = 1, Month = 1, Day = 1, Link = "notempty"}) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAValidPost_WhenITryAndDeleteThePost_ThenIGetRedirectedToPosts()
        {
            MockHttpContext.SetupProperty(h => h.User);
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);

            var model = new EditPostViewModel {BlogId = 1, PostId = 2};
            var result = (RedirectToRouteResult) controller.Delete(model);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Posts").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index").IgnoreCase);
        }

        [Test]
        public void GivenAValidPost_WhenITryAndDeleteThePost_ThenItIsDeleted()
        {
            MockHttpContext.SetupProperty(h => h.User);
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);

            var model = new EditPostViewModel {BlogId = 1, PostId = 2};
            controller.Delete(model);
            _postDomainMock.Verify(p => p.Delete(2), Times.Once());
        }

        [Test]
        public void GivenAValidPost_WhenITryAndEditAPost_ThenIGetTheCorrectView()
        {
            MockHttpContext.SetupProperty(h => h.User);
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);

            ActionResult result = controller.Edit(_userName, _blogId, 1);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void GivenAValidPost_WhenITryAndUpdateAPost_ThenIGetTheCorrectView()
        {
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);

            var result =
                controller.Update(new EditPostViewModel {Nickname = _userName, BlogId = _blogId, PostId = 1}) as
                RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Dashboard").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("index").IgnoreCase);
        }

        [Test]
        public void GivenAnErrorWhenAddingAComment_WhenIGetThePostWithTheComment_ThenTheViewDataIsUpdateWithTheError()
        {
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);
            var tempData = new ModelStateDictionary();
            tempData.Add("key", new ModelState());
            controller.TempData["comment"] = tempData;
            controller.Show(new PostLinkViewModel());

            Assert.That(controller.ViewData, Is.Not.Null);
            Assert.That(controller.ViewData.ModelState, Is.Not.Null);
            Assert.That(controller.ViewData.ModelState["key"], Is.Not.Null);
        }

        [Test]
        public void GivenAnInvalidPost_WhenITryAndDeleteThePost_ThenIGetRedirected()
        {
            MockHttpContext.SetupProperty(h => h.User);
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);
            controller.ModelState.AddModelError("Name", "Name error");

            var model = new EditPostViewModel {BlogId = 1, PostId = 2};
            var result = (ViewResult) controller.Delete(model);
            Assert.That(result.ViewName, Is.EqualTo("InvalidDelete"));
        }

        [Test]
        public void GivenAnInvalidPost_WhenITryAndUpdateAPost_ThenIHaveToReeditThePost()
        {
            var controller = new PostController(_postDomainMock.Object, _dashboardServiceMock, null);
            controller.ModelState.AddModelError("Name", "Name error");

            var result =
                controller.Update(new EditPostViewModel {Nickname = _userName, BlogId = _blogId, PostId = 1}) as
                ViewResult;

            Assert.That(result, Is.Not.Null);
        }
    }
}