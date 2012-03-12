using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlog.Models.Media;
using MBlogModel;
using NUnit.Framework;

namespace MBlogUnitTest.ViewModel
{
    [TestFixture]
    public class ShowMediaViewModelTest
    {
        [Test]
        public void WhenANewMediaViewModelIsCreated_ThenTheClassStringIsSetProperly()
        {
            Media media = new Media{Size = (int) Media.ValidSizes.Fullsize, Alignment = (int) Media.ValidAllignments.Left};
            ShowMediaViewModel model = new ShowMediaViewModel(media);
            Assert.That(model.ClassString, Is.EqualTo("img-fullsize img-align-left"));
        }

        [Test]
        public void GivenAFileWithAnExtension_ThenTheExtensionIsReturned()
        {
            Media media = new Media{FileName = "foo.exe"};
            ShowMediaViewModel model = new ShowMediaViewModel(media);
            Assert.That(model.Extension, Is.EqualTo("EXE"));
        }

        [Test]
        public void GivenAFileWithNoExtension_ThenTheUnknownFileTypeStringIsReturned()
        {
            Media media = new Media { FileName = "foo" };
            ShowMediaViewModel model = new ShowMediaViewModel(media);
            Assert.That(model.Extension, Is.EqualTo("Unknown File Type"));
        }

        [Test]
        public void GivenAMediaWithASetDate_ThenTheDisplayDateIsCorrect()
        {
            Media media = new Media { Year = 2010, Month = 3, Day = 2};
            ShowMediaViewModel model = new ShowMediaViewModel(media);
            Assert.That(model.DisplayDate, Is.EqualTo(new DateTime(2010, 3, 2).ToShortDateString()));
        }
    }
}
