using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MBlog.Models.Media;
using NUnit.Framework;

namespace MBlogUnitTest.ViewModel
{
    [TestFixture]
    public class NewMediaViewModelTest
    {
        [Test]
        public void GivenAFile_WhenItHasAnExtension_ThenTheExtensionIsRetrieved()
        {
            var model = new NewMediaViewModel();
            var extension = model.GetExtension("some.file.name.txt");

            Assert.That(extension, Is.EqualTo("txt"));
        }

        [Test]
        public void GivenAValidFIleExtension_ThenIsAllowedIsTrue()
        {
            var model = new NewMediaViewModel();
            Assert.IsTrue(model.IsAllowed("jpg"));
        }

        [Test]
        public void GivenAInvalidFIleExtension_ThenIsAllowedIsFalse()
        {
            var model = new NewMediaViewModel();
            Assert.IsFalse(model.IsAllowed("txt"));
        }
    }
}
