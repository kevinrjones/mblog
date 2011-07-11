using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlog.Models.Home;
using MBlog.Models.Post;
using NUnit.Framework;

namespace MBlogUnitTest.ViewModel
{
    [TestFixture]
    public class HomePagePostViewModelTest
    {
        [Test]
        public void GivenADefaultHomePagePostViewModel_WhenIAskForTheTitle_ThenItIsEmpty()
        {
            var model = new HomePagePostViewModel();
            Assert.That(model.Title, Is.StringMatching(""));
        }

        [Test]
        public void GivenADefaultHomePagePostViewModel_WhenIAskForThePost_ThenItIsEmpty()
        {

            var model = new HomePagePostViewModel();
            Assert.That(model.Post, Is.StringMatching(""));
        }

        [Test]
        public void GivenADefaultHomePagePostViewModel_WhenIAskForTheDate_ThenItIsEmpty()
        {

            var model = new HomePagePostViewModel();
            Assert.That(model.DatePosted, Is.EqualTo(new DateTime()));
        }


        [Test]
        public void GivenAHomePagePostViewModel_WhenIAskForTheTitle_ThenItIsTheValueInThePostViewModel()
        {
            PostViewModel postViewModel = new PostViewModel() { Title = "title", Post = "post", DatePosted = new DateTime(2011, 07, 11) };
            var model = new HomePagePostViewModel(postViewModel);
            Assert.That(model.Title, Is.StringMatching("title"));
        }

        [Test]
        public void GivenAHomePagePostViewModel_WhenIAskForThePost_ThenItIsTheValueInThePostViewModel()
        {
            PostViewModel postViewModel = new PostViewModel() { Title = "title", Post = "post", DatePosted = new DateTime(2011, 07, 11) };
            var model = new HomePagePostViewModel(postViewModel);
            Assert.That(model.Post, Is.StringMatching("post"));
        }

        [Test]
        public void GivenAHomePagePostViewModel_WhenIAskForTheDate_ThenItIsTheValueInThePostViewModel()
        {
            PostViewModel postViewModel = new PostViewModel() { Title = "title", Post = "post", DatePosted = new DateTime(2011, 07, 11) };
            var model = new HomePagePostViewModel(postViewModel);
            Assert.That(model.DatePosted, Is.EqualTo(new DateTime(2011, 07, 11)));
        }


    }
}
