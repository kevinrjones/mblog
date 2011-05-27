using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using MBlogIntegrationTest.Builder;
using MBlogModel;
using MBlogRepository.Repositories;
using NUnit.Framework;

namespace MBlogIntegrationTest
{
    [TestFixture]
    public class BlogRepositoryTest
    {
        private const int NumberOfPosts = 12;
        private User _user1;
        private User _user2;
        private TransactionScope _transactionScope;
        private string _nickname;
        private BlogRepository _blogRepository;
        private UserRepository _userRepository;
        Blog _blog;

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            _nickname = "nickname";

            List<Post> posts = new List<Post>();

            for (int i = 0; i < NumberOfPosts; i++)
            {
                Post post = BuildMeA.Post("title " + i, "entry " + i, DateTime.Today);
                posts.Add(post);
            }


            _blog = BuildMeA
                .Blog("title", "description", _nickname)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                              .WithBlog(_blog);

            Blog blog2 = BuildMeA
                .Blog("title", "description", "nickname2");

            _user2 = BuildMeA.User("email", "name", "password")
                              .WithBlog(blog2);

            _userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);
            _blogRepository = new BlogRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);

        }

        [Test]
        public void WhenIGetASpecificBlog_ThenIGetTheCorrectBlog()
        {
            _userRepository.Create(_user1);

            Blog blog = _blogRepository.GetBlog(_nickname);

            Assert.That(blog, Is.Not.Null);
            Assert.That(blog.Id, Is.EqualTo(_blog.Id));
        }

        //[Test]
        //public void WhenIGetABlog_ThenIGetTheBlogPosts()
        //{
        //    _userRepository.Create(_user1);

        //    Blog blog = _blogRepository.GetBlog(_nickname);

        //    Assert.That(blog, Is.Not.Null);
        //    Assert.That(blog.Posts.Count, Is.EqualTo(NumberOfPosts));
        //}

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }
    }
}
