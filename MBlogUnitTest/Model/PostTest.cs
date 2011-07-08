using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models;
using MBlog.Models.Validators;
using MBlogModel;
using MBlogRepository;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Model
{
    [TestFixture]
    public class PostTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void GivenAPost_WhenIInitializeAnEmptyInstance_ThenItIsNotValid()
        {
            Post post = new Post();
            ValidationContext ctx = new ValidationContext(post, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(post, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAPost_WhenItHasAnEmptyTitle_ThenItIsInValid()
        {
            Post post = new Post();
            post.AddPost("", "Post");
            ValidationContext ctx = new ValidationContext(post, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(post, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAPost_WhenItHasANullTitle_ThenItIsInValid()
        {
            Post post = new Post();
            post.AddPost(null, "Post");
            ValidationContext ctx = new ValidationContext(post, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(post, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAPost_WhenItHasANullBlogPost_ThenItIsInValid()
        {
            Post post = new Post();
            post.AddPost("Title", null);
            ValidationContext ctx = new ValidationContext(post, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(post, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAPost_WhenItHasAnEmptyBlogPost_ThenItIsInValid()
        {
            Post post = new Post();
            post.AddPost("Title", "");
            ValidationContext ctx = new ValidationContext(post, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(post, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAPost_WhenIIUpdateItAndtheTitleIsEmpty_ThenTheTitleDoesNotChange()
        {
            const string OriginalTitle = "OriginalTitle";
            Post post = new Post();
            post.AddPost(OriginalTitle, "Post");

            post.UpdatePost("", "New Post");

            Assert.That(post.Title, Is.EqualTo(OriginalTitle));
        }

        [Test]
        public void GivenAPost_WhenIIUpdateItAndtheTitleIsNull_ThenTheTitleDoesNotChange()
        {
            const string OriginalTitle = "OriginalTitle";
            Post post = new Post();
            post.AddPost(OriginalTitle, "Post");

            post.UpdatePost(null, "New Post");

            Assert.That(post.Title, Is.EqualTo(OriginalTitle));
        }

        [Test]
        public void GivenAPost_WhenIIUpdateItAndtheBlogPostIsEmpty_ThenTheBlogPostDoesNotChange()
        {
            const string OriginalPost = "OriginalPost";
            Post post = new Post();
            post.AddPost("title", OriginalPost);

            post.UpdatePost("New Title", "");

            Assert.That(post.BlogPost, Is.EqualTo(OriginalPost));
        }

        [Test]
        public void GivenAPost_WhenIIUpdateItAndtheBlogPostIsNull_ThenTheBlogPostDoesNotChange()
        {
            const string OriginalPost = "OriginalPost";
            Post post = new Post();
            post.AddPost("title", OriginalPost);

            post.UpdatePost("New Title", null);

            Assert.That(post.BlogPost, Is.EqualTo(OriginalPost));
        }

    }
}
