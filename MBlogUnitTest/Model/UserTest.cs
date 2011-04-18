using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models;
using MBlogModel;
using MBlogRepository;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Model
{
    [TestFixture]
    public class UserTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void GivenAUser_WhenIInitializeAnEmptyInstance_ThenItIsNotValid()
        {
            User user = new User();
            ValidationContext ctx = new ValidationContext(user, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(user, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAUser_WhenIInitializeItsProperties_ThenItIsValid()
        {
            User user = new User();
            user.AddUserDetails("Name", "EMail","hpass", false);
            ValidationContext ctx = new ValidationContext(user, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(user, ctx, validationResults);

            Assert.That(isValid, Is.True);
        }

    }
}
