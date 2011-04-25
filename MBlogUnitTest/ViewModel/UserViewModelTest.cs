using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using MBlog.Models;
using NUnit.Framework;

namespace MBlogUnitTest.ViewModel
{
    [TestFixture]
    public class UserViewModelTest
    {
        [Test]
        public void GivenAUserViewModel_WhenIAskForTheAuthenticationType_ThenItReturnsCookie()
        {
            UserViewModel viewModel = new UserViewModel();

            Assert.That(viewModel.AuthenticationType, Is.EqualTo("Cookie"));
        }

        [Test]
        public void GivenAUserViewModel_WhenIAskForTheRoles_ThenItThrowsAnException()
        {
            UserViewModel viewModel = new UserViewModel();

            Assert.Throws<NotImplementedException>(() => viewModel.IsInRole("any"));
        }

        [Test]
        public void GivenAUserViewModel_WhenITheUSerIsLoggedIn_ThenTheyAreAuthenticated()
        {
            UserViewModel viewModel = new UserViewModel {IsLoggedIn = true};

            Assert.That(viewModel.IsAuthenticated, Is.True);
        }

        [Test]
        public void GivenAUserViewModel_WhenITheUSerIsLoggedIn_ThenTheyAreNotAuthenticated()
        {
            UserViewModel viewModel = new UserViewModel { IsLoggedIn = false };

            Assert.That(viewModel.IsAuthenticated, Is.False);
        }

        [Test]
        public void GivenAUserViewModel_WhenIAskForItsIIdentity_ThenTItReturnsItself()
        {
            UserViewModel viewModel = new UserViewModel();

            IIdentity actual = viewModel.Identity;

            Assert.That(actual, Is.EqualTo(viewModel));
        }

        [Test]
        public void GivenAUserVIewModel_WhenThePassWordAndRepeatPasswrodAreTheSame_ThenItIsValid()
        {
            UserViewModel viewModel = new UserViewModel {Name = "Name", Email = "EMail"};
            viewModel.Password = "Password";
            viewModel.RepeatPassword = "Password";
            ValidationContext ctx = new ValidationContext(viewModel, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(viewModel, ctx, validationResults);

            Assert.That(isValid, Is.True);
        }

        [Test]
        public void GivenAUserVIewModel_WhenThePassWordAndRepeatPasswrodAreDifferent_ThenItIsNotValid()
        {
            UserViewModel viewModel = new UserViewModel { Name = "Name", Email = "EMail" };
            viewModel.Password = "Password";
            viewModel.RepeatPassword = "NotPassword";
            ValidationContext ctx = new ValidationContext(viewModel, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(viewModel, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAUserVIewModel_WhenTheNameIsMissing_ThenItIsNotValid()
        {
            UserViewModel viewModel = new UserViewModel { Email = "EMail"};
            ValidationContext ctx = new ValidationContext(viewModel, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(viewModel, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void GivenAUserVIewModel_WhenTheEMailIsMissing_ThenItIsNotValid()
        {
            UserViewModel viewModel = new UserViewModel { Name = "Name" };
            ValidationContext ctx = new ValidationContext(viewModel, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(viewModel, ctx, validationResults);

            Assert.That(isValid, Is.False);
        }

        // todo: move both test to UserViewMOdel tests
        [Test]
        public void GivenALoggedInUser_WhenThenBrowseToABlog_AndTheyAreNotTheOwner_ThenTheirIsOwnerFlagIsFalse()
        {
        }

        [Test]
        public void GivenALoggedInUser_WhenThenBrowseToABlog_AndTheyAreTheOwner_ThenTheirIsOwnerFlagIsTrue()
        {
        }

    }
}
