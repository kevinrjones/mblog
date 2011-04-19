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
    public class PostRepositoryTest
    {
        private User user;
        private TransactionScope _transactionScope;

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();

            List<Post> posts = new List<Post>();

            for (int i = 0; i < 12; i++)
            {
                Post post = BuildMeA.Post("title " + i, "entry " + i, DateTime.Today);
                posts.Add(post);
            }

            Blog blog = BuildMeA
                .Blog("title", "description", "nickname")
                .WithPosts(posts);

            user = BuildMeA.User("email", "name", "password")
                              .WithBlog(blog);
        }

        [Test]
        public void WhenIGetAllPosts_ThenIGetTheCorrectNumberOfEntries()
        {
            UserRepository userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);
            PostRepository postRepository = new PostRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);

            userRepository.Create(user);

            IEnumerable<Post> posts = postRepository.GetPosts();

            Assert.That(posts.Count(), Is.EqualTo(10));
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }
    }
}
