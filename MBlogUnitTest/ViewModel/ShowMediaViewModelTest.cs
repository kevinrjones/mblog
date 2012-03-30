using System;
using MBlog.Models.Media;
using MBlogModel;
using NUnit.Framework;

namespace MBlogUnitTest.ViewModel
{
    [TestFixture]
    public class ShowMediaViewModelTest
    {
        [Test]
        public void GivenAFileWithAnExtension_ThenTheExtensionIsReturned()
        {
            var media = new Media {FileName = "foo.exe"};
            var model = new ShowMediaViewModel(media);
            Assert.That(model.Extension, Is.EqualTo("EXE"));
        }

        [Test]
        public void GivenAFileWithNoExtension_ThenTheUnknownFileTypeStringIsReturned()
        {
            var media = new Media {FileName = "foo"};
            var model = new ShowMediaViewModel(media);
            Assert.That(model.Extension, Is.EqualTo("Unknown File Type"));
        }

        [Test]
        public void GivenAMediaWithASetDate_ThenTheDisplayDateIsCorrect()
        {
            var media = new Media {Year = 2010, Month = 3, Day = 2};
            var model = new ShowMediaViewModel(media);
            Assert.That(model.DisplayDate, Is.EqualTo(new DateTime(2010, 3, 2).ToShortDateString()));
        }

        [Test]
        public void WhenANewMediaViewModelIsCreated_ThenTheClassStringIsSetProperly()
        {
            var media = new Media
                            {Size = (int) Media.ValidSizes.Fullsize, Alignment = (int) Media.ValidAllignments.Left};
            var model = new ShowMediaViewModel(media);
            Assert.That(model.ClassString, Is.EqualTo("img-fullsize img-align-left"));
        }
    }
}