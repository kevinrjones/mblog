using MBlog.Models.Validators;
using NUnit.Framework;

namespace MBlogUnitTest.Model
{
    [TestFixture]
    public class ValidatorTest
    {
        [Test]
        public void GivenInvalidBBCode_WhenICallValidate_ThenItReturnsFalse()
        {
            var validator = new BBCodeValidator();
            bool actual = validator.IsValid("[b]Test");
            Assert.That(actual, Is.False);
        }

        [Test]
        public void GivenValidBBCode_WhenICallValidate_ThenItReturnsTrue()
        {
            var validator = new BBCodeValidator();
            bool actual = validator.IsValid("[b]Test[/b]");
            Assert.That(actual, Is.True);
        }
    }
}