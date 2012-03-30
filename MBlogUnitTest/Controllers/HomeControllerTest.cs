using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models.Home;
using MBlogModel;
using MBlogServiceInterfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    internal class HomeControllerTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _userDomain = new Mock<IUserService>();
            _postDomain = new Mock<IPostService>();

            InitializeUserRepository();

            var post = new Post
                           {
                               Posted = DateTime.Today,
                               BlogPost = "post",
                               Title = "title",
                               Blog = new Blog {User = new User {Name = "name"}}
                           };
            _postDomain.Setup(p => p.GetBlogPosts()).Returns(new List<Post> {post, post, post, post, post});

            _controller = new HomeController(_postDomain.Object, _userDomain.Object, null);
        }

        #endregion

        private Mock<IUserService> _userDomain;
        private Mock<IPostService> _postDomain;
        private HomeController _controller;

        private void InitializeUserRepository()
        {
            var userA = new User();
            userA.Blogs.Add(new Blog());
            userA.Blogs.Add(new Blog());
            userA.Blogs.Add(new Blog());
            var userB = new User();
            userB.Blogs.Add(new Blog());
            userB.Blogs.Add(new Blog());

            _userDomain.Setup(u => u.GetUsersWithTheirBlogs()).Returns(new List<User> {userA, userB, new User()});
        }

        [Test]
        public void GivenAHomeController_WhenIViewTheHomePage_AndThereAreNoUsers_ThenTheViewModelContainsNoUserEntries()
        {
            _userDomain.Setup(u => u.GetUsersWithTheirBlogs()).Returns(new List<User>());
            var result = _controller.Index() as ViewResult;

            var model = result.Model as HomePageViewModel;

            Assert.That(model.UserBlogViewModels.Count(), Is.EqualTo(0));
        }

        [Test]
        public void
            GivenAHomeController_WhenIViewTheHomePage_AndThereAreUsersButNoBlogs_ThenTheViewModelContainsNoUserEntries()
        {
            _userDomain.Setup(u => u.GetUsersWithTheirBlogs()).Returns(new List<User> {new User()});
            var result = _controller.Index() as ViewResult;

            var model = result.Model as HomePageViewModel;

            Assert.That(model.UserBlogViewModels.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GivenAHomeController_WhenIViewTheHomePage_ThenIGetTheIndexView()
        {
            var result = _controller.Index() as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAHomeController_WhenIViewTheHomePage_ThenTheCorectViewModelIsReturned()
        {
            var result = _controller.Index() as ViewResult;

            var model = result.Model as HomePageViewModel;

            Assert.That(model, Is.Not.Null);
        }

        [Test]
        public void GivenAHomeController_WhenIViewTheHomePage_ThenTheViewModelContainsTheCorrectNumberOfPosts()
        {
            var result = _controller.Index() as ViewResult;

            var model = result.Model as HomePageViewModel;

            Assert.That(model.HomePagePostViewModels.Count(), Is.EqualTo(5));
        }

        [Test]
        public void GivenAHomeController_WhenIViewTheHomePage_ThenTheViewModelContainsTheCorrectNumberOfUserEntries()
        {
            var result = _controller.Index() as ViewResult;

            var model = result.Model as HomePageViewModel;

            Assert.That(model.UserBlogViewModels.Count(), Is.EqualTo(5));
        }
    }
}