using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlog.Models.Post;
using NUnit.Framework;

namespace MBlogUnitTest.ViewModel
{
    [TestFixture]
    public class CreatePostViewModelTest
    {
        [Test]
        public void GivenAPostWithMalformedHTML_WhenItIsRetrievedForTheView_ThenTheHtmlIsWellFormed()
        {
            CreatePostViewModel model = new CreatePostViewModel();
            model.Post = "<p>Test";
            Assert.That(model.Post, Is.StringEnding("</p>"));
        }
    }
}
