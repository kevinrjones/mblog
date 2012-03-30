using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MBlogModel;
using NUnit.Framework;

namespace MBlogUnitTest.Model
{
    [TestFixture]
    public class UserTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
        }

        #endregion

        [Test]
        public void GivenAUser_WhenIInitializeAnEmptyInstance_ThenItIsNotValid()
        {
            var user = new User();
            var ctx = new ValidationContext(user, null, null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(user, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAUser_WhenIInitializeItsProperties_ThenItIsValid()
        {
            var user = new User("Name", "EMail", "hpass", false);
            var ctx = new ValidationContext(user, null, null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(user, ctx, validationResults);

            Assert.That(isValid, Is.True);
        }
    }
}