using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlogModel;
using NUnit.Framework;

namespace MBlogUnitTest.Model
{
    [TestFixture]
    class ImageTest
    {
        [Test]
        public void GivenImageDate_WhenICreateAnImage_ThenIGetValidDates()
        {
            var today = DateTime.Now;
            Image image = new Image("filename", "title", "caption", "description", "alternate", 1, "mime", "alignment", (int) Image.ValidSizes.Fullsize, new byte[]{});
            Assert.That(image.Year, Is.EqualTo(today.Year));
            Assert.That(image.Month, Is.EqualTo(today.Month));
            Assert.That(image.Day, Is.EqualTo(today.Day));
        }

        [Test]
        public void GivenAnInvalidSize_WhenICreateAnImage_ThenTheSizeIsSetToMedium()
        {
            Image image = new Image("filename", "title", "caption", "description", "alternate", 1, "mime", "alignment", 43, new byte[]{});
            Assert.That(image.Size, Is.EqualTo((int) Image.ValidSizes.Medium));
        }
    }
}
