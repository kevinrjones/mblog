using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using MBlog.Models.User;
using NUnit.Framework;

namespace MBlogUnitTest.ViewModel
{
    [TestFixture]
    public class UserViewModelTest
    {
        [Test]
        public void GivenANameAndAnId_ThenTheStringIsCorrectlyFormatted()
        {
            var viewModel = new UserViewModel {Name = "Fred", Id = 1};
            Assert.That(viewModel.ToString(), Is.EqualTo("Name: Fred, Id: 1"));
        }

        [Test]
        public void GivenAUserVIewModel_WhenTheEMailIsMissing_ThenItIsNotValid()
        {
            var viewModel = new UserViewModel {Name = "Name"};
            var ctx = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(viewModel, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAUserVIewModel_WhenTheNameIsMissing_ThenItIsNotValid()
        {
            var viewModel = new UserViewModel {Email = "EMail"};
            var ctx = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(viewModel, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAUserVIewModel_WhenThePassWordAndRepeatPasswrodAreDifferent_ThenItIsNotValid()
        {
            var viewModel = new UserViewModel {Name = "Name", Email = "EMail"};
            viewModel.Password = "Password";
            viewModel.RepeatPassword = "NotPassword";
            var ctx = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(viewModel, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAUserVIewModel_WhenThePassWordAndRepeatPasswrodAreTheSame_ThenItIsValid()
        {
            var viewModel = new UserViewModel {Name = "Name", Email = "EMail"};
            viewModel.Password = "Password";
            viewModel.RepeatPassword = "Password";
            var ctx = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(viewModel, ctx, validationResults);

            Assert.That(isValid, Is.True);
        }

        [Test]
        public void GivenAUserViewModel_WhenIAskForItsIIdentity_ThenTItReturnsItself()
        {
            var viewModel = new UserViewModel();

            IIdentity actual = viewModel.Identity;

            Assert.That(actual, Is.EqualTo(viewModel));
        }

        [Test]
        public void GivenAUserViewModel_WhenIAskForTheAuthenticationType_ThenItReturnsCookie()
        {
            var viewModel = new UserViewModel();

            Assert.That(viewModel.AuthenticationType, Is.EqualTo("Cookie"));
        }

        [Test]
        public void GivenAUserViewModel_WhenIAskForTheRoles_ThenItThrowsAnException()
        {
            var viewModel = new UserViewModel();

            Assert.Throws<NotImplementedException>(() => viewModel.IsInRole("any"));
        }

        [Test]
        public void GivenAUserViewModel_WhenITheUSerIsLoggedIn_ThenTheyAreAuthenticated()
        {
            var viewModel = new UserViewModel {IsLoggedIn = true};

            Assert.That(viewModel.IsAuthenticated, Is.True);
        }

        [Test]
        public void GivenAUserViewModel_WhenITheUSerIsLoggedIn_ThenTheyAreNotAuthenticated()
        {
            var viewModel = new UserViewModel {IsLoggedIn = false};

            Assert.That(viewModel.IsAuthenticated, Is.False);
        }
    }
}