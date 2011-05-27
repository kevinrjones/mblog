using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models;
using MBlogModel;
using MBlogRepository;
using MBlogRepository.Interfaces;
using MBlogRepository.Repositories;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    class HomeControllerTest
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IPostRepository> _postRepository;
        private HomeController _controller;

        [SetUp]
        public void SetUp()
        {
            _userRepository = new Mock<IUserRepository>();
            _postRepository = new Mock<IPostRepository>();

            InitializeUserRepository();

            var post = new Post { Posted = DateTime.Today, BlogPost = "post", Title = "title", Blog = new Blog{User = new User{Name = "name"}}};
            _postRepository.Setup(p => p.GetPosts()).Returns(new List<Post> { post, post, post, post, post });

            _controller = new HomeController(_userRepository.Object, _postRepository.Object);
        }

        private void InitializeUserRepository()
        {
            User userA = new User();
            userA.Blogs.Add(new Blog());
            userA.Blogs.Add(new Blog());
            userA.Blogs.Add(new Blog());
            User userB = new User();
            userB.Blogs.Add(new Blog());
            userB.Blogs.Add(new Blog());

            _userRepository.Setup(u => u.GetUsersWithTheirBlogs()).Returns(new List<User> { userA, userB, new User() });
        }

        [Test]
        public void GivenAHomeController_WhenIViewTheHomePage_ThenIGetTheIndexView()
        {
            ViewResult result = _controller.Index() as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAHomeController_WhenIViewTheHomePage_ThenTheCorectViewModelIsReturned()
        {
            ViewResult result = _controller.Index() as ViewResult;

            HomePageViewModel model = result.Model as HomePageViewModel;

            Assert.That(model, Is.Not.Null);
        }

        [Test]
        public void GivenAHomeController_WhenIViewTheHomePage_ThenTheViewModelContainsTheCorrectNumberOfUserEntries()
        {
            ViewResult result = _controller.Index() as ViewResult;

            HomePageViewModel model = result.Model as HomePageViewModel;

            Assert.That(model.UserBlogViewModels.Count(), Is.EqualTo(5));
        }

        [Test]
        public void GivenAHomeController_WhenIViewTheHomePage_AndThereAreNoUsers_ThenTheViewModelContainsNoUserEntries()
        {
            _userRepository.Setup(u => u.GetUsersWithTheirBlogs()).Returns(new List<User>());
            ViewResult result = _controller.Index() as ViewResult;

            HomePageViewModel model = result.Model as HomePageViewModel;

            Assert.That(model.UserBlogViewModels.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GivenAHomeController_WhenIViewTheHomePage_AndThereAreUsersButNoBlogs_ThenTheViewModelContainsNoUserEntries()
        {
            _userRepository.Setup(u => u.GetUsersWithTheirBlogs()).Returns(new List<User> { new User() });
            ViewResult result = _controller.Index() as ViewResult;

            HomePageViewModel model = result.Model as HomePageViewModel;

            Assert.That(model.UserBlogViewModels.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GivenAHomeController_WhenIViewTheHomePage_ThenTheViewModelContainsTheCorrectNumberOfPosts()
        {
            ViewResult result = _controller.Index() as ViewResult;

            HomePageViewModel model = result.Model as HomePageViewModel;

            Assert.That(model.HomePagePostViewModels.Count(), Is.EqualTo(5));
        }

    }
}
