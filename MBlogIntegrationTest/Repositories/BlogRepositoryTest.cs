using System;
using System.Collections.Generic;
using System.Configuration;
using System.Transactions;
using MBlogBuilder;
using MBlogModel;
using MBlogRepository.Repositories;
using NUnit.Framework;

namespace MBlogIntegrationTest.Repositories
{
    [TestFixture]
    public class BlogRepositoryTest
    {
        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            _nickname = "nickname";

            var posts = new List<Post>();

            for (int i = 0; i < NumberOfPosts; i++)
            {
                Post post = BuildMeA.Post("title " + i, "entry " + i, DateTime.Today, DateTime.Today);
                posts.Add(post);
            }


            _blog = BuildMeA
                .Blog("title", "description", _nickname, DateTime.Now)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                .WithBlog(_blog);

            Blog blog2 = BuildMeA
                .Blog("title", "description", "nickname2", DateTime.Now);

            _user2 = BuildMeA.User("email", "name", "password")
                .WithBlog(blog2);

            _userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["mblog"].ConnectionString);
            _blogRepository = new BlogRepository(ConfigurationManager.ConnectionStrings["mblog"].ConnectionString);
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }

        private const int NumberOfPosts = 12;
        private User _user1;
        private User _user2;
        private TransactionScope _transactionScope;
        private string _nickname;
        private BlogRepository _blogRepository;
        private UserRepository _userRepository;
        private Blog _blog;

        [Test]
        public void WhenIGetASpecificBlog_ThenIGetTheCorrectBlog()
        {
            _userRepository.Create(_user1);

            Blog blog = _blogRepository.GetBlog(_nickname);

            Assert.That(blog, Is.Not.Null);
            Assert.That(blog.Id, Is.EqualTo(_blog.Id));
        }

        [Test]
        public void WhenTheLastUpdateDateIsChanged_AndTheBlogDoesNotExist_ThenAnExceptionIsThrown()
        {
            _userRepository.Create(_user1);

            Blog blog = _blogRepository.GetBlog(_nickname);

            Assert.Throws<MBlogException>(() => _blogRepository.UpdateBlogStatistics(blog.Id + 1001));
        }

        [Test]
        public void WhenTheLastUpdateDateIsChanged_ThenTheNewValueIsRecorded()
        {
            _userRepository.Create(_user1);

            Blog blog = _blogRepository.GetBlog(_nickname);
            DateTime createdDateTime = blog.LastUpdated;

            _blogRepository.UpdateBlogStatistics(blog.Id);

            DateTime newTime = _blogRepository.GetBlog(blog.Nickname).LastUpdated;

            Assert.That(createdDateTime.Ticks, Is.Not.EqualTo(newTime.Ticks));
        }

        [Test]
        public void WhenAPostIsAdded_ThenTheTotalPostCountIsUpdated()
        {
            _userRepository.Create(_user1);

            Blog blog = _blogRepository.GetBlog(_nickname);
            int initialCount = blog.TotalPosts;

            _blogRepository.UpdateBlogStatistics(blog.Id);

            int newCount = _blogRepository.GetBlog(blog.Nickname).TotalPosts;

            Assert.That(newCount, Is.EqualTo(initialCount + 1));
        }
    }
}