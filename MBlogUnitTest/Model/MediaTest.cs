using System;
using MBlogModel;
using NUnit.Framework;

namespace MBlogUnitTest.Model
{
    [TestFixture]
    internal class MediaTest
    {
        [Test]
        public void GivenAnImageObject_ThenItIsHashCodeIsEqualToItsId()
        {
            var img = new Media {Id = 1};

            Assert.That(img.GetHashCode(), Is.EqualTo(1));
        }

        [Test]
        public void GivenAnImageObject_WhenItIsComparedToAnotherType_ThenTheyAreNotEqual()
        {
            var img = new Media();
            bool actual = img.Equals("string");
            Assert.That(actual, Is.False);
        }

        [Test]
        public void GivenAnImageObject_WhenItIsComparedToItself_AndItIsCastToObject_ThenTheyAreEqual()
        {
            var img = new Media();
            bool actual = img.Equals((object) img);
            Assert.That(actual, Is.True);
        }

        [Test]
        public void GivenAnImageObject_WhenItIsComparedToItself_ThenTheyAreEqual()
        {
            var img = new Media();
            bool actual = img.Equals(img);
            Assert.That(actual, Is.True);
        }

        [Test]
        public void GivenAnInvalidSize_WhenICreateAnImage_ThenTheSizeIsSetToFullsize()
        {
            var media = new Media("filename", "title", "caption", "description", "alternate", 1, "mime",
                                  (int) Media.ValidAllignments.None, 43, new byte[] {});
            Assert.That(media.Size, Is.EqualTo((int) Media.ValidSizes.Fullsize));
        }

        [Test]
        public void GivenImageData_WhenICreateAnImage_ThenIGetValidDates()
        {
            DateTime today = DateTime.Now;
            var media = new Media("filename", "title", "caption", "description", "alternate", 1, "mime",
                                  (int) Media.ValidAllignments.None, (int) Media.ValidSizes.Fullsize, new byte[] {});
            Assert.That(media.Year, Is.EqualTo(today.Year));
            Assert.That(media.Month, Is.EqualTo(today.Month));
            Assert.That(media.Day, Is.EqualTo(today.Day));
        }

        [Test]
        public void GivenTwoImageObjects_WhenOneIsNull_AndItIsCastToObject_ThenTheyAreNotEqual()
        {
            var img = new Media();
            bool actual = img.Equals((object) null);
            Assert.That(actual, Is.False);
        }

        [Test]
        public void GivenTwoImageObjects_WhenOneIsNull_ThenTheyAreNotEqual()
        {
            var img = new Media();
            bool actual = img.Equals(null);
            Assert.That(actual, Is.False);
        }

        [Test]
        public void WhenAUrlIsRequested_TheCorrectlyFormattedUrlIsReturned()
        {
            var media = new Media {Year = 2010, Month = 1, Day = 2, LinkKey = "link"};

            Assert.That(media.Url, Is.EqualTo("2010/1/2/link"));
        }

        [Test]
        public void WhenAnInvalidAlignmentIsSet_ThenTheAlignmentIsSetToNone()
        {
            var media = new Media();
            media.Alignment = 23;

            Assert.That(media.Alignment, Is.EqualTo(0));
        }
    }
}