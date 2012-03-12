using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models.Post;
using MBlogDomainInterfaces;
using MBlogModel;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    public class TestPostController : BaseControllerTests
    {
        Mock<IPostDomain> _postDomainMock;
        private IDashboardDomain _dashboardDomainMock;
        private string _userName = "UserName";
        private int _blogId = 10;

        List<Post> posts = new List<Post> { new Post { Title = "title1", BlogPost = ""}, new Post { Title = "title2" }, new Post { Title = "title3" } };

        [SetUp]
        public void SetUp()
        {

            _postDomainMock = new Mock<IPostDomain>();

            var dashboardDomainMock = new Mock<IDashboardDomain>();
            _dashboardDomainMock = dashboardDomainMock.Object;

            _postDomainMock.Setup(r => r.GetBlogPosts(It.IsAny<string>())).Returns(posts);
            _postDomainMock.Setup(r => r.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Post> { new Post { Id = 1, BlogPost = "empty", Title = "empty", Posted = DateTime.Today, Comments = new List<Comment> { new Comment { CommentText = "empty", Approved = true }, new Comment { CommentText = "empty", Approved = false } } } });
            _postDomainMock.Setup(r => r.GetBlogPost(It.IsAny<int>())).Returns(posts[0]);

        }

        [Test]
        public void GivenAPostController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectView()
        {
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);
            ViewResult result = (ViewResult)controller.Index("");

            PostsViewModel model = (PostsViewModel)result.Model;

            Assert.That(model.Posts.Count(), Is.EqualTo(posts.Count));
        }

        [Test]
        public void GivenAPostController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectNumberOfPosts()
        {
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);
            ViewResult result = (ViewResult)controller.Index("");

            PostsViewModel model = (PostsViewModel)result.Model;

            Assert.That(model.Posts.Count(), Is.EqualTo(posts.Count));
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_ThenItReturnsTheCorrectView()
        {
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);
            ViewResult result = controller.Show(new PostLinkViewModel()) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAPostController_WhenIIAskToShowASinglePost_ThenItReturnsTheCorrectView()
        {
            var postRepositoryMock = new Mock<IPostRepository>();

            postRepositoryMock.Setup(r => r.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<Post>{new Post{Title = "title"}});
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);
            ViewResult result = controller.Show(new PostLinkViewModel { Year = 1, Month = 1, Day = 1, Link = "notempty" }) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_ThenItReturnsTheCorrectModelInTheView()
        {
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);
            ViewResult result = controller.Show(new PostLinkViewModel()) as ViewResult;

            Assert.That(result.Model, Is.AssignableTo<PostsViewModel>());
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_AndIPassAYear_ThenItReturnsTheCorrectPosts()
        {
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);
            ViewResult result = controller.Show(new PostLinkViewModel { Year = 2010, Month = 0, Day = 0, Link = null }) as ViewResult;
            PostsViewModel model = (PostsViewModel)result.Model;

            Assert.That(model.Posts.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenAPostController_WhenIGetItsPostsWithComments_ThenItReturnsTheApprovedComments()
        {
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);
            ViewResult result = controller.Show(new PostLinkViewModel()) as ViewResult;
            PostsViewModel model = (PostsViewModel)result.Model;

            Assert.That(model.Posts.Count, Is.EqualTo(1));
            Assert.That(model.Posts[0].CommentCount, Is.EqualTo(1));
        }

        [Test]
        public void GivenAnErrorWhenAddingAComment_WhenIGetThePostWithTheComment_ThenTheViewDataIsUpdateWithTheError()
        {
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);
            var tempData = new ModelStateDictionary();
            tempData.Add("key", new ModelState());
            controller.TempData["comment"] = tempData;
            controller.Show(new PostLinkViewModel());

            Assert.That(controller.ViewData, Is.Not.Null);
            Assert.That(controller.ViewData.ModelState, Is.Not.Null);
            Assert.That(controller.ViewData.ModelState["key"], Is.Not.Null);
        }

        [Test]
        public void GivenAPostController_WhenICallItsNewMethod_ThenItReturnsTheCorrectView()
        {
            MockHttpContext.SetupProperty(h => h.User);

            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);
            var result = controller.New(_userName, _blogId) as ViewResult;

            Assert.That(result, Is.TypeOf<ViewResult>());            
        }

        [Test]
        public void GivenAPostController_WhenICallItsCreateMethod_AndTheModelIsValid_ThenItReturnsTheCorrectView()
        {
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);
            var result = controller.Create(new EditPostViewModel()) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("index").IgnoreCase);
        }

        [Test]
        public void GivenAPostController_WhenICallItsCreateMethod_AndTheModelIsInvalid_ThenItReturnsTheCorrectView()
        {
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);
            controller.ModelState.AddModelError("Title", "Title error");
            var result = controller.Create(new EditPostViewModel());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void GivenAValidPost_WhenITryAndEditAPost_ThenIGetTheCorrectView()
        {
            MockHttpContext.SetupProperty(h => h.User);
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);

            var result = controller.Edit(_userName, _blogId, 1);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void GivenAValidPost_WhenITryAndDeleteThePost_ThenItIsDeleted()
        {
            MockHttpContext.SetupProperty(h => h.User);
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);

            EditPostViewModel model = new EditPostViewModel { BlogId = 1, PostId = 2 };
            controller.Delete(model);
            _postDomainMock.Verify(p => p.Delete(2), Times.Once());
        }

        [Test]
        public void GivenAValidPost_WhenITryAndDeleteThePost_ThenIGetRedirectedToPosts()
        {
            MockHttpContext.SetupProperty(h => h.User);
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);

            EditPostViewModel model = new EditPostViewModel { BlogId = 1, PostId = 2 };
            RedirectToRouteResult result = (RedirectToRouteResult) controller.Delete(model);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Posts").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index").IgnoreCase);
        }

        [Test]
        public void GivenAnInvalidPost_WhenITryAndDeleteThePost_ThenIGetRedirected()
        {
            MockHttpContext.SetupProperty(h => h.User);
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);
            controller.ModelState.AddModelError("Name", "Name error");

            EditPostViewModel model = new EditPostViewModel { BlogId = 1, PostId = 2 };
            ViewResult result = (ViewResult) controller.Delete(model);
            Assert.That(result.ViewName, Is.EqualTo("InvalidDelete"));
            
        }

        [Test]
        public void GivenAValidPost_WhenITryAndUpdateAPost_ThenIGetTheCorrectView()
        {
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);

            RedirectToRouteResult result = controller.Update(new EditPostViewModel { Nickname = _userName, BlogId = _blogId, PostId = 1 }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Dashboard").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("index").IgnoreCase);
        }

        [Test]
        public void GivenAnInvalidPost_WhenITryAndUpdateAPost_ThenIHaveToReeditThePost()
        {
            PostController controller = new PostController(_postDomainMock.Object, _dashboardDomainMock, null);
            controller.ModelState.AddModelError("Name", "Name error");

            ViewResult result = controller.Update(new EditPostViewModel { Nickname = _userName, BlogId = _blogId, PostId = 1 }) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

    }
}
