using System;
using System.Collections.Generic;
using MBlog.Models.Post;
using NUnit.Framework;

namespace MBlogUnitTest.ViewModel
{
    [TestFixture]
    public class PostViewModelTest
    {
        [Test]
        public void GivenANewPostsCollection_WhenIAccessTheCollection_ThenItIsTheSameAsTheSetValue()
        {
            List<PostViewModel> posts = new List<PostViewModel>();
            PostsViewModel model = new PostsViewModel();
            model.Posts = posts;
            Assert.That(model.Posts, Is.EqualTo(posts));
        }

        [Test]
        public void GivenAStringDate_WhenIAskForTheDate_ThenTheCorrectValueIsReturned()
        {
            PostViewModel model = new PostViewModel();
            model.DatePosted = new DateTime(2011, 07, 11);
            Assert.That(model.DatePosted, Is.EqualTo(new DateTime(2011, 07, 11)));
            Assert.That(model.YearPosted, Is.EqualTo("2011"));
            Assert.That(model.MonthPosted, Is.EqualTo("07"));
            Assert.That(model.DayPosted, Is.EqualTo("11"));
        }

    }
}