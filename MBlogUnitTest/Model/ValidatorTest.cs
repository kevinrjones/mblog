using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlog.Models.Validators;
using NUnit.Framework;

namespace MBlogUnitTest.Model
{
    [TestFixture]
    public class ValidatorTest
    {
        [Test]
        public void GivenValidBBCode_WhenICallValidate_ThenItReturnsTrue()
        {
            BBCodeValidator validator = new BBCodeValidator();
            bool actual = validator.IsValid("[b]Test[/b]");
            Assert.That(actual, Is.True);
        }

        [Test]
        public void GivenInvalidBBCode_WhenICallValidate_ThenItReturnsFalse()
        {
            BBCodeValidator validator = new BBCodeValidator();
            bool actual = validator.IsValid("[b]Test");
            Assert.That(actual, Is.False);
        }
    }
}
