using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Transactions;
using MBlogModel;
using MBlogRepository.Repositories;
using MBogIntegrationTest.Builder;
using NUnit.Framework;

namespace MBogIntegrationTest
{

    [TestFixture]
    public class UserRepositoryTest
    {
        private User user;
        private TransactionScope _transactionScope;
        private Post _post;
        private string _nickname1;
        private string _nickname2;
        UserRepository userRepository;
        BlogPostRepository postRepository;

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            _nickname1 = "nickname1";
            _nickname2 = "nickname2";

            userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);
            postRepository = new BlogPostRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);

            Blog blog1 = BuildMeA
                .Blog("title1", "description1", _nickname1);
            Blog blog2 = BuildMeA
                .Blog("title2", "description2", _nickname1);

            user = BuildMeA.User("email1", "name1", "password1")
                              .WithBlog(blog1)
                              .WithBlog(blog2);

            userRepository.Create(user);

            User user2 = BuildMeA.User("email1", "name1", "password1");
            userRepository.Create(user2);
        }

        [Test]
        public void WhenIGetASpecificUserById_ThenIGetTheCorrectUser()
        {
            User newUser = userRepository.GetUser(user.Id);

            Assert.That(newUser, Is.Not.Null);
            Assert.That(newUser.Id, Is.EqualTo(user.Id));
        }

        [Test]
        public void WhenIGetASpecificUserByEMail_ThenIGetTheCorrectUser()
        {
            User newUser = userRepository.GetUser(user.Email);

            Assert.That(newUser, Is.Not.Null);
            Assert.That(newUser.Id, Is.EqualTo(user.Id));
        }

        [Test]
        public void WhenIGetAUserAndAllBlogs_ThenIGetTheCorrectBlogs()
        {
            IEnumerable<User> users = userRepository.GetUsersWithTheirBlogs();

            Assert.That(users, Is.Not.Null);
            Assert.That(users.Count(), Is.EqualTo(2));
            
            var  selected = (from u in users
                             where u.Id == user.Id
                            select u).FirstOrDefault();

            Assert.That(selected.Blogs.Count, Is.EqualTo(2));
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }
    }
}
