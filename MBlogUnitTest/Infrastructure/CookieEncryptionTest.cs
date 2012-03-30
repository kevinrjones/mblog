using System;
using MBlog.Infrastructure;
using NUnit.Framework;

namespace MBlogUnitTest.Infrastructure
{
    [TestFixture]
    public class CookieEncryptionTest
    {
        [Test]
        public void GivenANullCipherText_WhenITryAndDecryptIt_ThenIGetAnException()
        {
            Assert.Throws<ArgumentNullException>(() => ((byte[]) null).Decrypt());
        }

        [Test]
        public void GivenANullString_WhenITryAndEncryptIt_ThenIGetAnException()
        {
            Assert.Throws<ArgumentNullException>(() => ((string) null).Encrypt());
        }

        [Test]
        public void GivenAZeroLengthCipherText_WhenITryAndDecryptIt_ThenIGetAnException()
        {
            var cipherText = new byte[0];
            Assert.Throws<ArgumentNullException>(() => cipherText.Decrypt());
        }

        [Test]
        public void GivenAnEmptyString_WhenITryAndEncryptAString_ThenIGetAnException()
        {
            Assert.Throws<ArgumentNullException>(() => "".Encrypt());
        }
    }
}