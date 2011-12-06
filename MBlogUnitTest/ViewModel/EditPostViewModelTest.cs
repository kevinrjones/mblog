using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlog.Models.Post;
using NUnit.Framework;

namespace MBlogUnitTest.ViewModel
{
    [TestFixture]
    public class EditPostViewModelTest
    {
        [Test]
        public void GivenAPostWithMalformedHTML_WhenItIsRetrievedForTheView_ThenTheHtmlIsWellFormed()
        {
            EditPostViewModel model = new EditPostViewModel();
            model.Post = "<span>Test";
            Assert.That(model.Post, Is.StringEnding("</span>"));
        }
    }
}
