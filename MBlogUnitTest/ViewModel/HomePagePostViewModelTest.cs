﻿using System;
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
        public void GivenADefaultHomePagePostViewModel_WhenIAskForTheDate_ThenItIsEmpty()
        {
            var model = new HomePagePostViewModel();
            Assert.That(model.DatePosted, Is.EqualTo(new DateTime()));
        }

        [Test]
        public void GivenADefaultHomePagePostViewModel_WhenIAskForThePost_ThenItIsEmpty()
        {
            var model = new HomePagePostViewModel();
            Assert.That(model.Post, Is.StringMatching(""));
        }

        [Test]
        public void GivenADefaultHomePagePostViewModel_WhenIAskForTheTitle_ThenItIsEmpty()
        {
            var model = new HomePagePostViewModel();
            Assert.That(model.Title, Is.StringMatching(""));
        }


        [Test]
        public void GivenAHomePagePostViewModel_WhenIAskForATruncatedPost_ThenItReturnsWellFormedHtml()
        {
            var builder = new StringBuilder();
            builder.Append("<span>");
            for (int i = 0; i < 300; i++)
            {
                builder.Append("a");
            }
            string span = "</span>";
            builder.Append(span);
            int actualLength = HomePagePostViewModel.MaxLength + span.Length;
            var postViewModel = new PostViewModel
                                    {
                                        Title = "title",
                                        Post = builder.ToString(),
                                        DatePosted = new DateTime(2011, 07, 11)
                                    };
            var model = new HomePagePostViewModel(postViewModel);
            Assert.That(model.Post, Is.StringEnding(span));
            Assert.That(model.Post.Length, Is.EqualTo(actualLength));
        }

        [Test]
        public void GivenAHomePagePostViewModel_WhenIAskForTheDate_ThenItIsTheValueInThePostViewModel()
        {
            var postViewModel = new PostViewModel
                                    {Title = "title", Post = "post", DatePosted = new DateTime(2011, 07, 11)};
            var model = new HomePagePostViewModel(postViewModel);
            Assert.That(model.DatePosted, Is.EqualTo(new DateTime(2011, 07, 11)));
        }

        [Test]
        public void
            GivenAHomePagePostViewModel_WhenIAskForThePostAndItIsLessThen200Characters_ThenItIsTheValueInThePostViewModel
            ()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < 200; i++)
            {
                builder.Append("a");
            }

            var postViewModel = new PostViewModel
                                    {
                                        Title = "title",
                                        Post = builder.ToString(),
                                        DatePosted = new DateTime(2011, 07, 11)
                                    };
            var model = new HomePagePostViewModel(postViewModel);
            Assert.That(model.Post, Is.StringMatching(builder.ToString()));
        }

        [Test]
        public void
            GivenAHomePagePostViewModel_WhenIAskForThePostAndItIsMoreThenMaxCharacters_ThenItReturns200Characters()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < HomePagePostViewModel.MaxLength + 100; i++)
            {
                builder.Append("a");
            }

            var postViewModel = new PostViewModel
                                    {
                                        Title = "title",
                                        Post = builder.ToString(),
                                        DatePosted = new DateTime(2011, 07, 11)
                                    };
            var model = new HomePagePostViewModel(postViewModel);
            Assert.That(model.Post.Length, Is.EqualTo(HomePagePostViewModel.MaxLength));
        }

        [Test]
        public void GivenAHomePagePostViewModel_WhenIAskForThePost_ThenItIsTheValueInThePostViewModel()
        {
            var postViewModel = new PostViewModel
                                    {Title = "title", Post = "post", DatePosted = new DateTime(2011, 07, 11)};
            var model = new HomePagePostViewModel(postViewModel);
            Assert.That(model.Post, Is.StringMatching("post"));
        }

        [Test]
        public void GivenAHomePagePostViewModel_WhenIAskForTheTitle_ThenItIsTheValueInThePostViewModel()
        {
            var postViewModel = new PostViewModel
                                    {Title = "title", Post = "post", DatePosted = new DateTime(2011, 07, 11)};
            var model = new HomePagePostViewModel(postViewModel);
            Assert.That(model.Title, Is.StringMatching("title"));
        }
    }
}