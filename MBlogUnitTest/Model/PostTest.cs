using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MBlogModel;
using NUnit.Framework;

namespace MBlogUnitTest.Model
{
    [TestFixture]
    public class PostTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
        }

        #endregion

        [Test]
        public void GivenAPost_WhenIIUpdateItAndtheBlogPostIsEmpty_ThenTheBlogPostDoesNotChange()
        {
            const string OriginalPost = "OriginalPost";
            var post = new Post();
            post.AddPost("title", OriginalPost);

            post.UpdatePost("New Title", "");

            Assert.That(post.BlogPost, Is.EqualTo(OriginalPost));
        }

        [Test]
        public void GivenAPost_WhenIIUpdateItAndtheBlogPostIsNull_ThenTheBlogPostDoesNotChange()
        {
            const string OriginalPost = "OriginalPost";
            var post = new Post();
            post.AddPost("title", OriginalPost);

            post.UpdatePost("New Title", null);

            Assert.That(post.BlogPost, Is.EqualTo(OriginalPost));
        }

        [Test]
        public void GivenAPost_WhenIIUpdateItAndtheTitleIsEmpty_ThenTheTitleDoesNotChange()
        {
            const string OriginalTitle = "OriginalTitle";
            var post = new Post();
            post.AddPost(OriginalTitle, "Post");

            post.UpdatePost("", "New Post");

            Assert.That(post.Title, Is.EqualTo(OriginalTitle));
        }

        [Test]
        public void GivenAPost_WhenIIUpdateItAndtheTitleIsNull_ThenTheTitleDoesNotChange()
        {
            const string OriginalTitle = "OriginalTitle";
            var post = new Post();
            post.AddPost(OriginalTitle, "Post");

            post.UpdatePost(null, "New Post");

            Assert.That(post.Title, Is.EqualTo(OriginalTitle));
        }

        [Test]
        public void GivenAPost_WhenIInitializeAnEmptyInstance_ThenItIsNotValid()
        {
            var post = new Post();
            var ctx = new ValidationContext(post, null, null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(post, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAPost_WhenItHasANullBlogPost_ThenItIsInValid()
        {
            var post = new Post();
            post.AddPost("Title", null);
            var ctx = new ValidationContext(post, null, null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(post, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAPost_WhenItHasANullTitle_ThenItIsInValid()
        {
            var post = new Post();
            post.AddPost(null, "Post");
            var ctx = new ValidationContext(post, null, null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(post, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAPost_WhenItHasAnEmptyBlogPost_ThenItIsInValid()
        {
            var post = new Post();
            post.AddPost("Title", "");
            var ctx = new ValidationContext(post, null, null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(post, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAPost_WhenItHasAnEmptyTitle_ThenItIsInValid()
        {
            var post = new Post();
            post.AddPost("", "Post");
            var ctx = new ValidationContext(post, null, null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(post, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }
    }
}