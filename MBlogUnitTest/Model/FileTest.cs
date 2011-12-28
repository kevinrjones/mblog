using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlogModel;
using NUnit.Framework;

namespace MBlogUnitTest.Model
{
    [TestFixture]
    class FileTest
    {
        [Test]
        public void GivenImageData_WhenICreateAnImage_ThenIGetValidDates()
        {
            var today = DateTime.Now;
            Media media = new Media("filename", "title", "caption", "description", "alternate", 1, "mime", (int) Media.ValidAllignments.None, (int) Media.ValidSizes.Fullsize, new byte[]{});
            Assert.That(media.Year, Is.EqualTo(today.Year));
            Assert.That(media.Month, Is.EqualTo(today.Month));
            Assert.That(media.Day, Is.EqualTo(today.Day));
        }

        [Test]
        public void GivenAnInvalidSize_WhenICreateAnImage_ThenTheSizeIsSetToMedium()
        {
            Media media = new Media("filename", "title", "caption", "description", "alternate", 1, "mime", (int) Media.ValidAllignments.None, 43, new byte[]{});
            Assert.That(media.Size, Is.EqualTo((int) Media.ValidSizes.Medium));
        }
    }
}
