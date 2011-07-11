using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MBlog.Infrastructure;

namespace MBlogUnitTest.Infrastructure
{
    [TestFixture]
    public class CookieEncryptionTest
    {
        [Test]
        public void GivenANullString_WhenITryAndEncryptIt_ThenIGetAnException()
        {            
            Assert.Throws<ArgumentNullException>(() => ((string) null).Encrypt());
        }

        [Test]
        public void GivenAnEmptyString_WhenITryAndEncryptAString_ThenIGetAnException()
        {
            Assert.Throws<ArgumentNullException>(() => "".Encrypt());
        }

        [Test]
        public void GivenANullCipherText_WhenITryAndDecryptIt_ThenIGetAnException()
        {
            Assert.Throws<ArgumentNullException>(() => ((byte[])null).Decrypt());
        }

        [Test]
        public void GivenAZeroLengthCipherText_WhenITryAndDecryptIt_ThenIGetAnException()
        {
            byte[] cipherText = new byte[0];
            Assert.Throws<ArgumentNullException>(() => cipherText.Decrypt());
        }
    }
}
