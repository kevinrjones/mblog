using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models;
using MBlog.Models.Post;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository;
using MBlogRepository.Interfaces;
using MBlogRepository.Repositories;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    public class TestPostController : BaseControllerTests
    {
        private IPostRepository _postRepositoryMock;
        private IBlogRepository _blogRepositoryMock;
        private IUserRepository _userRepositoryMock;
        private string _userName = "UserName";
        private int _blogId = 10;

        List<Post> posts = new List<Post> { new Post { Title = "title1", BlogPost = ""}, new Post { Title = "title2" }, new Post { Title = "title3" } };

        [SetUp]
        public void SetUp()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            _userRepositoryMock = userRepositoryMock.Object;

            var postRepositoryMock = new Mock<IPostRepository>();
            _postRepositoryMock = postRepositoryMock.Object;

            postRepositoryMock.Setup(r => r.GetBlogPosts(It.IsAny<string>())).Returns(posts);
            postRepositoryMock.Setup(r => r.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Post> { new Post { Id = 1, BlogPost = "empty", Title = "empty", Posted = DateTime.Today, Comments = new List<Comment> { new Comment { CommentText = "empty", Approved = true }, new Comment { CommentText = "empty", Approved = false } } } });
            postRepositoryMock.Setup(r => r.GetBlogPost(It.IsAny<int>())).Returns(posts[0]);

            var mockBlog = new Mock<IBlogRepository>();
            _blogRepositoryMock = mockBlog.Object;

            mockBlog.Setup(b => b.GetBlog(It.IsAny<string>())).Returns(new Blog());
        }

        [Test]
        public void GivenAPostController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectView()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, null);
            ViewResult result = (ViewResult)controller.Index("");

            PostsViewModel model = (PostsViewModel)result.Model;

            Assert.That(model.Posts.Count(), Is.EqualTo(posts.Count));
        }

        [Test]
        public void GivenAPostController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectNumberOfPosts()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, null);
            ViewResult result = (ViewResult)controller.Index("");

            PostsViewModel model = (PostsViewModel)result.Model;

            Assert.That(model.Posts.Count(), Is.EqualTo(posts.Count));
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_ThenItReturnsTheCorrectView()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, null);
            ViewResult result = controller.Show(new PostLinkViewModel()) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAPostController_WhenIIAskToShowASinglePost_ThenItReturnsTheCorrectView()
        {
            var postRepositoryMock = new Mock<IPostRepository>();

            postRepositoryMock.Setup(r => r.GetBlogPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<Post>{new Post{Title = "title"}});
            PostController controller = new PostController(_blogRepositoryMock, postRepositoryMock.Object, null);
            ViewResult result = controller.Show(new PostLinkViewModel{Year = 1, Month = 1, Day = 1, Link = "notempty"}) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_ThenItReturnsTheCorrectModelInTheView()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, null);
            ViewResult result = controller.Show(new PostLinkViewModel()) as ViewResult;

            Assert.That(result.Model, Is.AssignableTo<PostsViewModel>());
        }

        [Test]
        public void GivenAPostController_WhenICallItsShowMethod_AndIPassAYear_ThenItReturnsTheCorrectPosts()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, null);
            ViewResult result = controller.Show(new PostLinkViewModel { Year = 2010, Month = 0, Day = 0, Link = null }) as ViewResult;
            PostsViewModel model = (PostsViewModel)result.Model;

            Assert.That(model.Posts.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenAPostController_WhenIGetItsPostsWithComments_ThenItReturnsTheApprovedComments()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, null);
            ViewResult result = controller.Show(new PostLinkViewModel()) as ViewResult;
            PostsViewModel model = (PostsViewModel)result.Model;

            Assert.That(model.Posts.Count, Is.EqualTo(1));
            Assert.That(model.Posts[0].CommentCount, Is.EqualTo(1));
        }

        [Test]
        public void GivenAnErrorWhenAddingAComment_WhenIGetThePostWithTheComment_ThenTheViewDataIsUpdateWithTheError()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, null);
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
            var mockBlog = new Mock<IBlogRepository>();
            _blogRepositoryMock = mockBlog.Object;

            MockHttpContext.SetupProperty(h => h.User);
            
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, _userRepositoryMock);
            SetControllerContext(controller);
            var result = controller.New(_userName, _blogId) as ViewResult;

            Assert.That(result, Is.TypeOf<ViewResult>());            
        }

        [Test]
        public void GivenAPostController_WhenICallItsCreateMethod_AndTheModelIsValid_ThenItReturnsTheCorrectView()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, _userRepositoryMock);
            SetControllerContext(controller);
            var result = controller.Create(new CreatePostViewModel()) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("index").IgnoreCase);
        }

        [Test]
        public void GivenAPostController_WhenICallItsCreateMethod_AndTheModelIsInvalid_ThenItReturnsTheCorrectView()
        {
            PostController controller = new PostController(_blogRepositoryMock, _postRepositoryMock, _userRepositoryMock);
            SetControllerContext(controller);
            controller.ModelState.AddModelError("Title", "Title error");
            var result = controller.Create(new CreatePostViewModel());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void GivenAValidPost_WhenITryAndEditAPost_ThenIGetTheCorrectView()
        {
            var mockBlog = new Mock<IBlogRepository>();
            MockHttpContext.SetupProperty(h => h.User);
            PostController controller = new PostController(mockBlog.Object, _postRepositoryMock, _userRepositoryMock);
            SetControllerContext(controller);

            var result = controller.Edit(_userName, _blogId, 1);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void GivenAValidPost_WhenITryAndUpdateAPost_ThenIGetTheCorrectView()
        {
            var mockBlog = new Mock<IBlogRepository>();
            mockBlog.Setup(b => b.GetBlog(_userName)).Returns(new Blog { Id = _blogId });
            PostController controller = new PostController(mockBlog.Object, _postRepositoryMock, _userRepositoryMock);
            SetControllerContext(controller);

            RedirectToRouteResult result = controller.Update(new EditPostViewModel { Nickname = _userName, BlogId = _blogId, PostId = 1 }) as RedirectToRouteResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Admin").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("index").IgnoreCase);
        }

        [Test]
        public void GivenAnInvalid_WhenITryAndUpdateAPost_ThenIGetTheCorrectView()
        {
            var mockBlog = new Mock<IBlogRepository>();
            mockBlog.Setup(b => b.GetBlog(_userName)).Returns(new Blog { Id = _blogId });
            PostController controller = new PostController(mockBlog.Object, _postRepositoryMock, _userRepositoryMock);
            controller.ModelState.AddModelError("Name", "Name error");
            SetControllerContext(controller);

            ViewResult result = controller.Update(new EditPostViewModel { Nickname = _userName, BlogId = _blogId, PostId = 1 }) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAnInValidPost_WhenITryAndUpdateThePost_ThenIGetAnException()
        {
            var mockBlog = new Mock<IBlogRepository>();
            mockBlog.Setup(b => b.GetBlog(_userName)).Returns(new Blog { Id = _blogId });
            var postRepositoryMock = new Mock<IPostRepository>();
            PostController controller = new PostController(mockBlog.Object, postRepositoryMock.Object, _userRepositoryMock);
            SetControllerContext(controller);

            Assert.Throws<MBlogException>(() => controller.Update(new EditPostViewModel { PostId = 1, BlogId = _blogId, Nickname = _userName }));

        }
    }
}
