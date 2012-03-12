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

        [Test]
        public void GivenAQQFile_ThenTheCorrectMimeTypeIsReturned()
        {
            var model = new NewMediaViewModel {QqFile = "name.jpg"};
            Assert.That(model.ContentType, Is.EqualTo("image/jpeg"));
        }

        [Test]
        public void GivenAValidQqFileExtension_ThenTheModelIsValid()
        {
            var model = new NewMediaViewModel {QqFile = "name.jpg"};
            Assert.That(model.Validate(null).Count(), Is.EqualTo(0));
        }

        [Test]
        public void GivenAValidFileExtension_ThenTheModelIsValid()
        {
            var model = new NewMediaViewModel { File = new TestHttpPostedFileBase("foo.jpg") };
            Assert.That(model.Validate(null).Count(), Is.EqualTo(0));
        }

        [Test]
        public void GivenAInvalidFileExtension_ThenTheModelIsInvalid()
        {
            var model = new NewMediaViewModel { File = new TestHttpPostedFileBase("foo.bar") };
            Assert.That(model.Validate(null).Count(), Is.EqualTo(1));
        }
    }

    public class TestHttpPostedFileBase : HttpPostedFileBase
    {
        private string _fileName;
        public TestHttpPostedFileBase(string fileName)
        {
            _fileName = fileName;
        }

        public override string FileName
        {
            get
            {
                return _fileName;
            }
        }
    }
}
