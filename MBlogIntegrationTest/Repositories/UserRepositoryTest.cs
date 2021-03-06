﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using MBlogBuilder;
using MBlogModel;
using MBlogRepository.Repositories;
using NUnit.Framework;

namespace MBlogIntegrationTest.Repositories
{
    [TestFixture]
    public class UserRepositoryTest
    {
        private User _user;
        private TransactionScope _transactionScope;
        private string _nickname1;
        private UserRepository _userRepository;
        private User _user2;


        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            _nickname1 = "nickname1";

            _userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["mblog"].ConnectionString);

            Blog blog1 = BuildMeA
                .Blog("title1", "description1", _nickname1, DateTime.Now);
            Blog blog2 = BuildMeA
                .Blog("title2", "description2", _nickname1, DateTime.Now);

            _user = BuildMeA.User("email1", "name1", "password1")
                .WithBlog(blog1)
                .WithBlog(blog2);

            _userRepository.Create(_user);

            _user2 = BuildMeA.User("email1", "name1", "password1");
            _userRepository.Create(_user2);
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }

        [Test]
        public void GivenMoreThanOneUser_WhenIGetAskForAllUsersAndBlogs_ThenIGetAllUsersAndBlogs()
        {
            List<User> users = _userRepository.GetUsersWithTheirBlogs().ToList();

            Assert.That(users[0].Blogs.Count, Is.EqualTo(2));
            Assert.That(users[1].Blogs.Count, Is.EqualTo(0));
        }

        [Test]
        public void GivenMoreThanOneUser_WhenIGetAskForAllUsers_ThenIGetAllUsers()
        {
            IEnumerable<User> users = _userRepository.GetUsers();

            Assert.That(users, Is.Not.Null);
            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void WhenIGetASpecificUserAndTheirBlogsById_ThenIGetTheCorrectUserAndTheirBlogs()
        {
            _userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["mblog"].ConnectionString);
            User newUser = _userRepository.GetUserWithTheirBlogs(_user.Id);

            Assert.That(newUser.Id, Is.EqualTo(_user.Id));
            Assert.That(newUser.Blogs.Count, Is.EqualTo(2));
        }

        [Test]
        public void WhenIGetASpecificUserByEMail_ThenIGetTheCorrectUser()
        {
            User newUser = _userRepository.GetUser(_user2.Email);

            Assert.That(newUser, Is.Not.Null);
            Assert.That(newUser.Id, Is.EqualTo(_user.Id));
        }

        [Test]
        public void WhenIGetASpecificUserById_ThenIGetTheCorrectUser()
        {
            User newUser = _userRepository.GetUser(_user.Id);

            Assert.That(newUser, Is.Not.Null);
            Assert.That(newUser.Id, Is.EqualTo(_user.Id));
        }

        [Test]
        public void WhenIGetAUserAndAllBlogs_ThenIGetTheCorrectBlogs()
        {
            IEnumerable<User> users = _userRepository.GetUsersWithTheirBlogs();

            Assert.That(users, Is.Not.Null);
            Assert.That(users.Count(), Is.EqualTo(2));

            User selected = (from u in users
                             where u.Id == _user.Id
                             select u).FirstOrDefault();

            Assert.That(selected.Blogs.Count, Is.EqualTo(2));
        }
    }
}