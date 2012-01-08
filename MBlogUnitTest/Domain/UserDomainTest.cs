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
        private UserDomain _userDomain;
        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _blacklistRepository = new Mock<IUsernameBlacklistRepository>();
            _userDomain = new UserDomain(_userRepository.Object, _blacklistRepository.Object, null);
        }

        [Test]
        public void GivenAValidEmail_WhenAUserIsRequested_ThenTheUserIsReturned()
        {
            _userRepository.Setup(u => u.GetUser(_email)).Returns(new User { Email = _email });
            User user = _userDomain.GetUser(_email);
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Email, Is.EqualTo(_email));
        }

        [Test]
        public void GivenAnInvalidEmail_WhenAUserIsRequested_ThenTheUserIsReturned()
        {
            _userRepository.Setup(u => u.GetUser(_email)).Returns(new User());
            User user = _userDomain.GetUser(_email);
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Email, Is.Null);
        }

        [Test]
        public void GivenAnEmail_WhenTheRepositoryThrowsAnException_ThenAnMBlogExceptionIsReThrown()
        {
            _userRepository.Setup(u => u.GetUser(It.IsAny<string>())).Throws<Exception>();
            Assert.Throws<MBlogException>(() => _userDomain.GetUser(It.IsAny<string>()));
        }

        [Test]
        public void GivenValidDetails_WhenAUserIsCreated_ThenAValidUserIsReturned()
        {
            User user = _userDomain.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.That(user, Is.Not.Null);
        }

        [Test]
        public void GivenValidDetails_WhenAUserIsCreated_ThenTheUserAddedToTheRepository()
        {
            _userDomain.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            _userRepository.Verify(u => u.Create(It.IsAny<User>()), Times.Once());
        }

        [Test]
        public void GivenDetails_WhenAUserIsCreated_AndAnExceptionIsThrown_ThenAnMBlogExceptionIsRethrown()
        {
            _userRepository.Setup(u => u.Create(It.IsAny<User>())).Throws<Exception>();
            Assert.Throws<MBlogException>(() => _userDomain.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void GivenAnInvalidEmail_WhenTheValidityOfTheUserIsChecked_ThenTheErrorDetailsContainsTheError()
        {
            _userRepository.Setup(u => u.GetUser(It.IsAny<string>())).Returns(new User());
            _blacklistRepository.Setup(b => b.GetName(It.IsAny<string>())).Returns((Blacklist) null);
            var errors = _userDomain.IsUserRegistrationValid(It.IsAny<string>(), It.IsAny<string>());

            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0].FieldName, Is.EqualTo("email").IgnoreCase);
        }

        [Test]
        public void GivenABlacklistedName_WhenTheValidityOfTheUserIsChecked_ThenTheErrorDetailsContainsTheError()
        {
            _userRepository.Setup(u => u.GetUser(It.IsAny<string>())).Returns((User) null);
            _blacklistRepository.Setup(b => b.GetName(It.IsAny<string>())).Returns(new Blacklist());
            var errors = _userDomain.IsUserRegistrationValid(It.IsAny<string>(), It.IsAny<string>());

            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0].FieldName, Is.EqualTo("name").IgnoreCase);
        }

        [Test]
        public void GivenABlacklistedNameAndAnInvalidEmail_WhenTheValidityOfTheUserIsChecked_ThenTheErrorDetailsContainsTheErrors()
        {
            _userRepository.Setup(u => u.GetUser(It.IsAny<string>())).Returns(new User());
            _blacklistRepository.Setup(b => b.GetName(It.IsAny<string>())).Returns(new Blacklist());
            var errors = _userDomain.IsUserRegistrationValid(It.IsAny<string>(), It.IsAny<string>());

            Assert.That(errors.Count, Is.EqualTo(2));
        }

        [Test]
        public void GivenAnUnavailableRepository_WhenTheValidityOfTheUserIsChecked_ThenAnMBlogExceptionIsThrown()
        {
            _userRepository.Setup(u => u.GetUser(It.IsAny<string>())).Throws<Exception>();
            Assert.Throws<MBlogException>(()=> _userDomain.IsUserRegistrationValid(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void GivenAValidUser_WhenTheirBlogsAreRetrieved_ThenAllTheBlogsAreReturned()
        {
            _userRepository.Setup(u => u.GetUserWithTheirBlogs(1)).Returns(new User{Id = 1});
            User user = _userDomain.GetUserWithTheirBlogs(1);
            Assert.That(user.Id, Is.EqualTo(1));
        }

        [Test]
        public void GivenAValidUser_WhenTheirBlogsAreRetrieved_AndTheDatabaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _userRepository.Setup(u => u.GetUserWithTheirBlogs(It.IsAny<int>())).Throws<Exception>();
            Assert.Throws<MBlogException>(() => _userDomain.GetUserWithTheirBlogs(1));
        }

        [Test]
        public void WhenAllBlogsAreRetrieved_ThenAllTheBlogsAreReturned()
        {
            _userRepository.Setup(u => u.GetUsersWithTheirBlogs()).Returns(new List<User>{ new User { Id = 1 }});
            var users = _userDomain.GetUsersWithTheirBlogs().ToList();
            Assert.That(users[0].Id, Is.EqualTo(1));
        }

        [Test]
        public void WhenAllBlogsAreRetrieved_AndTheDatabaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _userRepository.Setup(u => u.GetUsersWithTheirBlogs()).Throws<Exception>();
            Assert.Throws<MBlogException>(() => _userDomain.GetUsersWithTheirBlogs());
        }

    }
}
