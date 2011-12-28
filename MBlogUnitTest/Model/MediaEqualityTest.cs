using MBlogModel;
using NUnit.Framework;

namespace MBlogUnitTest.Model
{
    [TestFixture]
    internal class MediaEqualityTest
    {
        [Test]
        public void GivenTwoImageObjects_WhenOneIsNull_ThenTheyAreNotEqual()
        {
            Media img = new Media();
            bool actual = img.Equals(null);
            Assert.That(actual, Is.False);
        }

        [Test]
        public void GivenTwoImageObjects_WhenOneIsNull_AndItIsCastToObject_ThenTheyAreNotEqual()
        {
            Media img = new Media();
            bool actual = img.Equals((object)null);
            Assert.That(actual, Is.False);
        }

        [Test]
        public void GivenAnImageObject_WhenItIsComparedToItself_ThenTheyAreEqual()
        {
            Media img = new Media();
            bool actual = img.Equals(img);
            Assert.That(actual, Is.True);
        }

        [Test]
        public void GivenAnImageObject_WhenItIsComparedToItself_AndItIsCastToObject_ThenTheyAreEqual()
        {
            Media img = new Media();
            bool actual = img.Equals((object)img);
            Assert.That(actual, Is.True);
        }

        [Test]
        public void GivenAnImageObject_WhenItIsComparedToAnotherType_ThenTheyAreNotEqual()
        {
            Media img = new Media();
            bool actual = img.Equals("string");
            Assert.That(actual, Is.False);
        }


    }
}