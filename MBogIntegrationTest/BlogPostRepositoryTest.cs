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
    public class BlogPostRepositoryTest
    {
        private User _user1;
        private User _user2;
        private TransactionScope _transactionScope;
        private Post _post1;
        private string _nickname;
        UserRepository userRepository;
        BlogPostRepository postRepository;

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            _nickname = "nickname";

            _post1 = BuildMeA.Post("Title", "Entry", DateTime.Today);

            Blog blog1 = BuildMeA
                .Blog("title", "description", _nickname)
                .WithPost(_post1);

            _user1 = BuildMeA.User("email", "name", "password")
                              .WithBlog(blog1);

            Post _post2 = BuildMeA.Post("Title", "Entry", DateTime.Today);

            Blog blog2 = BuildMeA
                .Blog("title", "description", "nickname2")
                .WithPost(_post2);

            _user2 = BuildMeA.User("email", "name", "password")
                              .WithBlog(blog2);
            userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);
            postRepository = new BlogPostRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);
        }

        [Test]
        public void WhenIGetASpecificPost_ThenIGetTheCorrectPost()
        {
            userRepository.Create(_user1);

            Post newPost = postRepository.GetBlogPost(_post1.Id);

            Assert.That(newPost, Is.Not.Null);
            Assert.That(newPost.Id, Is.EqualTo(_post1.Id));
        }

        [Test]
        public void GivenTheUserHasCreatedOnePosts_WhenIGetAllPosts_ThenIGetTheCorrectPosts()
        {
            userRepository.Create(_user1);

            IEnumerable<Post> newPosts = postRepository.GetBlogPosts(_nickname);

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GivenTheUserHasCreatedMultiplePosts_WhenIGetAllPosts_ThenIGetTheCorrectPosts()
        {
            List<Post> posts = new List<Post> { BuildMeA.Post("title 1", "entry 1", DateTime.Today), BuildMeA.Post("title 1", "entry 1", DateTime.Today), };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                              .WithBlog(blog);

            userRepository.Create(_user1);

            IEnumerable<Post> newPosts = postRepository.GetBlogPosts(_nickname);

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(posts.Count));
        }

        [Test]
        public void GivenTheUserHasCreatedNoPosts_WhenIGetAllPosts_ThenIGetTheCorrectPosts()
        {
            Blog blog = BuildMeA
                .Blog("title", "description", _nickname);

            _user1 = BuildMeA.User("email", "name", "password")
                              .WithBlog(blog);

            userRepository.Create(_user1);

            IEnumerable<Post> newPosts = postRepository.GetBlogPosts(_nickname);

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GivenTheUserHasCreatedMultiplePostsOnTheSmaeDate_WhenIGetAllPosts_ThenIGetTheCorrectPosts()
        {
            List<Post> posts = new List<Post> { BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 20)), 
            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                              .WithBlog(blog);

            List<Post> posts2 = new List<Post> { BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 20)), 
            };

            Blog blog2 = BuildMeA
                .Blog("title2", "description2", "nickname2")
                .WithPosts(posts2);

            _user2 = BuildMeA.User("email2", "name2", "password2")
                              .WithBlog(blog2);

            userRepository.Create(_user1);
            userRepository.Create(_user2);

            IEnumerable<Post> newPosts = postRepository.GetBlogPosts(2011, 4, 19, _nickname, null);

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(2));
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }
    }
}
