using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlogDomain;
using MBlogModel;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Domain
{
    [TestFixture]
    public class UserDomainTest
    {
        private const string _email = "email";
        Mock<IUserRepository> _userRepository;
        Mock<IUsernameBlacklistRepository> _blacklistRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _blacklistRepository = new Mock<IUsernameBlacklistRepository>();
        }

        [Test]
        public void GivenAValidEmail_WhenAUserIsRequested_ThenTheUserIsReturned()
        {
            var userDomain = new UserDomain(_userRepository.Object, _blacklistRepository.Object, null);
            _userRepository.Setup(u => u.GetUser(_email)).Returns(new User { Email = _email });
            User user = userDomain.GetUser(_email);
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Email, Is.EqualTo(_email));
        }

        [Test]
        public void GivenAnInvalidEmail_WhenAUserIsRequested_ThenTheUserIsReturned()
        {
            var userDomain = new UserDomain(_userRepository.Object, _blacklistRepository.Object, null);
            _userRepository.Setup(u => u.GetUser(_email)).Returns(new User());
            User user = userDomain.GetUser(_email);
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Email, Is.Null);
        }

        [Test]
        public void GivenAnEmail_WhenTheRepositoryThrowsAnException_ThenAnMBlogExceptionIsReThrown()
        {
            var userDomain = new UserDomain(_userRepository.Object, _blacklistRepository.Object, null);
            _userRepository.Setup(u => u.GetUser(It.IsAny<string>())).Throws<Exception>();
            Assert.Throws<MBlogException>(() => userDomain.GetUser(It.IsAny<string>()));
        }

        [Test]
        public void GivenValidDetails_WhenAUserIsCreated_ThenAValidUserIsReturned()
        {
            var userDomain = new UserDomain(_userRepository.Object, _blacklistRepository.Object, null);
            User user = userDomain.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.That(user, Is.Not.Null);
        }

        [Test]
        public void GivenValidDetails_WhenAUserIsCreated_ThenTheUserAddedToTheRepository()
        {
            var userDomain = new UserDomain(_userRepository.Object, _blacklistRepository.Object, null);
            userDomain.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            _userRepository.Verify(u => u.Create(It.IsAny<User>()), Times.Once());
        }

        [Test]
        public void GivenDetails_WhenAUserIsCreated_AndAnExceptionIsThrown_ThenAnMBlogExceptionIsRethrown()
        {
            var userDomain = new UserDomain(_userRepository.Object, _blacklistRepository.Object, null);
            _userRepository.Setup(u => u.Create(It.IsAny<User>())).Throws<Exception>();
            Assert.Throws<MBlogException>(() => userDomain.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void GivenAnInvalidEmail_WhenTheValidityOfTheUserIsChecked_ThenTheErrorDetailsContainsTheError()
        {
            var userDomain = new UserDomain(_userRepository.Object, _blacklistRepository.Object, null);
            _userRepository.Setup(u => u.GetUser(It.IsAny<string>())).Returns((User)null);
            _blacklistRepository.Setup(b => b.GetName(It.IsAny<string>())).Returns(new Blacklist());
            var errors = userDomain.IsUserRegistrationValid(It.IsAny<string>(), It.IsAny<string>());

            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0].FieldName, Is.EqualTo("email").IgnoreCase);
        }

        [Test]
        public void GivenABlacklistedName_WhenTheValidityOfTheUserIsChecked_ThenTheErrorDetailsContainsTheError()
        {
            var userDomain = new UserDomain(_userRepository.Object, _blacklistRepository.Object, null);
            _userRepository.Setup(u => u.GetUser(It.IsAny<string>())).Returns(new User());
            _blacklistRepository.Setup(b => b.GetName(It.IsAny<string>())).Returns((Blacklist)null);
            var errors = userDomain.IsUserRegistrationValid(It.IsAny<string>(), It.IsAny<string>());

            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0].FieldName, Is.EqualTo("name").IgnoreCase);
        }

        [Test]
        public void GivenABlacklistedNameAndAnInvalidEmail_WhenTheValidityOfTheUserIsChecked_ThenTheErrorDetailsContainsTheErrors()
        {
            var userDomain = new UserDomain(_userRepository.Object, _blacklistRepository.Object, null);
            _userRepository.Setup(u => u.GetUser(It.IsAny<string>())).Returns((User)null);
            _blacklistRepository.Setup(b => b.GetName(It.IsAny<string>())).Returns((Blacklist)null);
            var errors = userDomain.IsUserRegistrationValid(It.IsAny<string>(), It.IsAny<string>());

            Assert.That(errors.Count, Is.EqualTo(2));
        }

        [Test]
        public void GivenAnUnavailableRepository_WhenTheValidityOfTheUserIsChecked_ThenAnMBlogExceptionIsThrown()
        {
            var userDomain = new UserDomain(_userRepository.Object, _blacklistRepository.Object, null);
            _userRepository.Setup(u => u.GetUser(It.IsAny<string>())).Throws<Exception>();
            Assert.Throws<MBlogException>(()=> userDomain.IsUserRegistrationValid(It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}
