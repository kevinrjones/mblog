using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using MBlogIntegrationTest.Builder;
using MBlogModel;
using MBlogRepository.Repositories;
using NUnit.Framework;

namespace MBlogIntegrationTest
{
    [TestFixture]
    public class PostRepositoryTest
    {
        private User _user1;
        private User _user2;
        private TransactionScope _transactionScope;
        private Post _post1;
        private string _nickname;
        UserRepository _userRepository;
        PostRepository _postRepository;
        private Blog _blog1;

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            _nickname = "nickname";

            _post1 = BuildMeA.Post("Title", "Entry", DateTime.Today);

            _blog1 = BuildMeA
                .Blog("title", "description", _nickname)
                .WithPost(_post1);

            _user1 = BuildMeA.User("email", "name", "password")
                              .WithBlog(_blog1);

            Post _post2 = BuildMeA.Post("Title", "Entry", DateTime.Today);

            Blog blog2 = BuildMeA
                .Blog("title", "description", "nickname2")
                .WithPost(_post2);

            _user2 = BuildMeA.User("email", "name2", "password")
                              .WithBlog(blog2);

            List<Post> posts = new List<Post>();    

            _userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);
            _postRepository = new PostRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);
        }

        [Test]
        public void WhenIGetASpecificPost_ThenIGetTheCorrectPost()
        {
            _userRepository.Create(_user1);

            Post newPost = _postRepository.GetBlogPost(_post1.Id);

            Assert.That(newPost, Is.Not.Null);
            Assert.That(newPost.Id, Is.EqualTo(_post1.Id));
        }

        [Test]
        public void WhenIGetASpecificPost_ThenIGetTheCorrectUser()
        {
            _userRepository.Create(_user1);

            Post newPost = _postRepository.GetBlogPost(_post1.Id);

            Assert.That(newPost, Is.Not.Null);
            Assert.That(newPost.Blog.User.Name, Is.EqualTo("name"));
        }

        [Test]
        public void GivenASetOfBlogPosts_WhenIGetAllPosts_ThenIGetTheCorrectPosts()
        {
            _userRepository.Create(_user1);
            _userRepository.Create(_user2);

            IEnumerable<Post> newPosts = _postRepository.GetBlogPosts(null);

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GivenASetOfBlogPosts_WhenIGetAllPosts_ThenIAlsoGetTheUsers()
        {
            _userRepository.Create(_user1);

            IList<Post> newPosts = _postRepository.GetBlogPosts(_nickname);

            Assert.That(newPosts[0].Blog.User.Name, Is.EqualTo("name"));
        }

        [Test]
        public void GivenTheUserHasCreatedOnePosts_WhenIGetAllPosts_ThenIGetTheCorrectPosts()
        {
            _userRepository.Create(_user1);

            IEnumerable<Post> newPosts = _postRepository.GetBlogPosts(_nickname);

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

            _userRepository.Create(_user1);

            IEnumerable<Post> newPosts = _postRepository.GetBlogPosts(_nickname);

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

            _userRepository.Create(_user1);

            IEnumerable<Post> newPosts = _postRepository.GetBlogPosts(_nickname);

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GivenTheUserHasCreatedMultiplePostsOnTheSameDate_WhenIGetAllPosts_ThenIGetTheCorrectPosts()
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

            _userRepository.Create(_user1);
            _userRepository.Create(_user2);

            IEnumerable<Post> newPosts = _postRepository.GetBlogPosts(2011, 4, 19, _nickname, null);

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GivenTheUserHasMultiplePosts_WhenIGetAllPostsForThatNickname_ThenIGetTheCorrectPosts()
        {
            List<Post> posts = new List<Post> { BuildMeA.Post("title 1", "entry 1", new DateTime(2010, 4, 19)), 
                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 5, 20)), 
                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 6, 21)), 
            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                              .WithBlog(blog);

            List<Post> posts2 = new List<Post> { BuildMeA.Post("title 2", "entry 1", new DateTime(2010, 4, 18)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 3, 19)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 20)), 
            };

            Blog blog2 = BuildMeA
                .Blog("title2", "description2", "nickname2")
                .WithPosts(posts2);

            _user2 = BuildMeA.User("email2", "name2", "password2")
                              .WithBlog(blog2);

            _userRepository.Create(_user1);
            _userRepository.Create(_user2);

            IEnumerable<Post> newPosts = _postRepository.GetBlogPosts(0, 0, 0, _nickname, null);

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(4));
        }

        [Test]
        public void GivenTheUserHasMultiplePostsForTheSameYear_WhenIGetAllPostsForThatYear_ThenIGetTheCorrectPosts()
        {
            List<Post> posts = new List<Post> { BuildMeA.Post("title 1", "entry 1", new DateTime(2010, 4, 19)), 
                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 5, 20)), 
                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 6, 21)), 
            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                              .WithBlog(blog);

            List<Post> posts2 = new List<Post> { BuildMeA.Post("title 2", "entry 1", new DateTime(2010, 4, 18)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 3, 19)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 20)), 
            };

            Blog blog2 = BuildMeA
                .Blog("title2", "description2", "nickname2")
                .WithPosts(posts2);

            _user2 = BuildMeA.User("email2", "name2", "password2")
                              .WithBlog(blog2);

            _userRepository.Create(_user1);
            _userRepository.Create(_user2);

            IEnumerable<Post> newPosts = _postRepository.GetBlogPosts(2011, 0, 0, _nickname, null);

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GivenTheUserHasMultiplePostsForTheSameYearAndMonth_WhenIGetAllPostsForThatYearAndMonth_ThenIGetTheCorrectPosts()
        {
            List<Post> posts = new List<Post> { BuildMeA.Post("title 1", "entry 1", new DateTime(2010, 4, 19)), 
                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 20)), 
                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 6, 21)), 
            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                              .WithBlog(blog);

            List<Post> posts2 = new List<Post> { BuildMeA.Post("title 2", "entry 1", new DateTime(2010, 4, 19)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 20)), 
            };

            Blog blog2 = BuildMeA
                .Blog("title2", "description2", "nickname2")
                .WithPosts(posts2);

            _user2 = BuildMeA.User("email2", "name2", "password2")
                              .WithBlog(blog2);

            _userRepository.Create(_user1);
            _userRepository.Create(_user2);

            IEnumerable<Post> newPosts = _postRepository.GetBlogPosts(2011, 4, 0, _nickname, null);

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GivenTheUserHasMultiplePostsForTheSameYearMonthAndDay_WhenIGetAllPostsForThatYearMonthAndDay_ThenIGetTheCorrectPosts()
        {
            List<Post> posts = new List<Post> { BuildMeA.Post("title 1", "entry 1", new DateTime(2010, 4, 19)), 
                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 6, 21)), 
            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                              .WithBlog(blog);

            List<Post> posts2 = new List<Post> { BuildMeA.Post("title 2", "entry 1", new DateTime(2010, 4, 19)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 20)), 
            };

            Blog blog2 = BuildMeA
                .Blog("title2", "description2", "nickname2")
                .WithPosts(posts2);

            _user2 = BuildMeA.User("email2", "name2", "password2")
                              .WithBlog(blog2);

            _userRepository.Create(_user1);
            _userRepository.Create(_user2);

            IEnumerable<Post> newPosts = _postRepository.GetBlogPosts(2011, 4, 19, _nickname, null);

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GivenTheUserHasMultiplePostsForTheSameYearMonthDayAndTitle_WhenIGetAllPostsForThatYearMonthDayAndTitle_ThenIGetTheCorrectPosts()
        {
            List<Post> posts = new List<Post> { BuildMeA.Post("title 1", "entry 1", new DateTime(2010, 4, 19)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 3", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 4", "entry 1", new DateTime(2011, 6, 21)), 
            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                              .WithBlog(blog);

            List<Post> posts2 = new List<Post> { BuildMeA.Post("title 2", "entry 1", new DateTime(2010, 4, 19)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 20)), 
            };

            Blog blog2 = BuildMeA
                .Blog("title2", "description2", "nickname2")
                .WithPosts(posts2);

            _user2 = BuildMeA.User("email2", "name2", "password2")
                              .WithBlog(blog2);

            _userRepository.Create(_user1);
            _userRepository.Create(_user2);

            IEnumerable<Post> newPosts = _postRepository.GetBlogPosts(2011, 4, 19, _nickname, "title-2");

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GivenTheUserHasMultiplePostsForTheSameYearMonthDayAndTitle_WhenIGetAllPostsForThatYearMonthDayWithTheWrongTitleTitle_ThenIGetNoPosts()
        {
            List<Post> posts = new List<Post> { BuildMeA.Post("title 1", "entry 1", new DateTime(2010, 4, 19)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 3", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 4", "entry 1", new DateTime(2011, 6, 21)), 
            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                              .WithBlog(blog);

            List<Post> posts2 = new List<Post> { BuildMeA.Post("title 2", "entry 1", new DateTime(2010, 4, 19)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19)), 
                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 20)), 
            };

            Blog blog2 = BuildMeA
                .Blog("title2", "description2", "nickname2")
                .WithPosts(posts2);

            _user2 = BuildMeA.User("email2", "name2", "password2")
                              .WithBlog(blog2);

            _userRepository.Create(_user1);
            _userRepository.Create(_user2);

            IEnumerable<Post> newPosts = _postRepository.GetBlogPosts(2011, 4, 19, _nickname, "title-3");

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GivenThereIsAPost_WhenIAddAComment_TheCommentIsAdded()
        {
            _userRepository.Create(_user1);
            Post post = _postRepository.AddComment(_post1.Id, "CommentName", "Comment Text");

            Assert.That(post.Comments.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenThereIsAComment_WhenIRetriveTheOwningPost_ThenCommentIsRetrived()
        {
            Comment comment = BuildMeA.Comment("This is a comment", DateTime.Now);

            List<Post> posts = new List<Post> { BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19)).WithComment(comment)};

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                              .WithBlog(blog);
            _userRepository.Create(_user1);
            Post newPost = _postRepository.GetBlogPost(posts[0].Id);
            Assert.That(newPost.Comments.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenANewPost_WhenIAddThePostToTheDatabase_AndTheBlogDoesNotExist_ThenItIsAdded()
        {
            _userRepository.Create(_user1);
            Post post = new Post { Title = "Title", BlogPost = "Post", Edited = DateTime.UtcNow, Posted = DateTime.UtcNow, BlogId = 10 };
            
            Assert.Throws<DbUpdateException>(() => _postRepository.Create(post));
        }

        [Test]
        public void GivenANewPost_WhenIAddThePostToTheDatabase_ThenItIsAdded()
        {
            _userRepository.Create(_user1);
            Post post = new Post { Title = "Title", BlogPost = "Post", Edited = DateTime.UtcNow, Posted = DateTime.UtcNow, BlogId = _blog1.Id };

            _postRepository.Create(post);

            Assert.That(post.Id, Is.Not.EqualTo(0));
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }
    }
}
