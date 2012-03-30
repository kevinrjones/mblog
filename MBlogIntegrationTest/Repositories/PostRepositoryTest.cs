using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
using MBlogBuilder;
using MBlogModel;
using MBlogRepository.Repositories;
using NUnit.Framework;

namespace MBlogIntegrationTest.Repositories
{
    [TestFixture]
    public class PostRepositoryTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            _nickname = "nickname";

            _post1 = BuildMeA.Post("Title", "Entry", DateTime.Today, DateTime.Today);

            _blog1 = BuildMeA
                .Blog("title", "description", _nickname, DateTime.Now)
                .WithPost(_post1);

            _user1 = BuildMeA.User("email", "name", "password")
                .WithBlog(_blog1);

            _post2 = BuildMeA.Post("Title", "Entry", DateTime.Today, DateTime.Today, false);

            Blog blog2 = BuildMeA
                .Blog("title", "description", "nickname2", DateTime.Now)
                .WithPost(_post2);

            _user2 = BuildMeA.User("email", "name2", "password")
                .WithBlog(blog2);

            _userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["mblog"].ConnectionString);
            _postRepository = new PostRepository(ConfigurationManager.ConnectionStrings["mblog"].ConnectionString);
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }

        #endregion

        private User _user1;
        private User _user2;
        private TransactionScope _transactionScope;
        private Post _post1;
        private string _nickname;
        private UserRepository _userRepository;
        private PostRepository _postRepository;
        private Blog _blog1;
        private Post _post2;

        [Test]
        public void GivenABlogId_WhenIAskForItsOrderedPosts_ThenIGetThePostsInDateOrder()
        {
            var posts = new List<Post>
                            {
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2000, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19), DateTime.Today)
                            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname, DateTime.Now)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                .WithBlog(blog);
            _userRepository.Create(_user1);

            var newPosts = _postRepository.GetOrderedBlogPosts(blog.Id) as List<Post>;

            Assert.That(newPosts[0].Title, Is.StringEnding("2"));
        }

        [Test]
        public void GivenABlogId_WhenIAskForItsPosts_ThenIGetAllThePosts()
        {
            _userRepository.Create(_user1);
            IList<Post> posts = _postRepository.GetBlogPosts(_blog1.Id);

            Assert.That(posts.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenANewPost_WhenIAddThePostToTheDatabase_AndTheBlogDoesNotExist_ThenThePostIsNotAdded()
        {
            _userRepository.Create(_user1);
            var post = new Post
                           {
                               Title = "Title",
                               BlogPost = "Post",
                               Edited = DateTime.UtcNow,
                               Posted = DateTime.UtcNow,
                               BlogId = 10
                           };

            Assert.Throws<DbUpdateException>(() => _postRepository.Create(post));
        }

        [Test]
        public void GivenANewPost_WhenIAddThePostToTheDatabase_ThenItIsAdded()
        {
            _userRepository.Create(_user1);
            var post = new Post
                           {
                               Title = "Title",
                               BlogPost = "Post",
                               Edited = DateTime.UtcNow,
                               Posted = DateTime.UtcNow,
                               BlogId = _blog1.Id
                           };

            _postRepository.Create(post);

            Assert.That(post.Id, Is.Not.EqualTo(0));
        }


        [Test]
        public void GivenASetOfBlogPosts_WhenIGetAllPostsForANulllUser_ThenIGetTheCorrectPosts()
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
        public void GivenASetOfBlogPosts_WhenIGetAllPosts_ThenIGetTheCorrectPosts()
        {
            _userRepository.Create(_user1);
            _userRepository.Create(_user2);

            IEnumerable<Post> newPosts = _postRepository.GetPosts();

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GivenAnInvalidBlogId_WhenIAskForItsPosts_ThenIGetNoPosts()
        {
            IList<Post> posts = _postRepository.GetBlogPosts(_blog1.Id);

            Assert.That(posts.Count, Is.EqualTo(0));
        }

        [Test]
        public void GivenTheUserHasCreatedMultiplePostsOnTheSameDate_WhenIGetAllPosts_ThenIGetTheCorrectPosts()
        {
            var posts = new List<Post>
                            {
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 20), DateTime.Today),
                            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname, DateTime.Now)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                .WithBlog(blog);

            var posts2 = new List<Post>
                             {
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 20), DateTime.Today),
                             };

            Blog blog2 = BuildMeA
                .Blog("title2", "description2", "nickname2", DateTime.Now)
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
        public void GivenTheUserHasCreatedMultiplePosts_WhenIGetAllPosts_ThenIGetTheCorrectPosts()
        {
            var posts = new List<Post>
                            {
                                BuildMeA.Post("title 1", "entry 1", DateTime.Today, DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", DateTime.Today, DateTime.Today),
                            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname, DateTime.Now)
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
                .Blog("title", "description", _nickname, DateTime.Now);

            _user1 = BuildMeA.User("email", "name", "password")
                .WithBlog(blog);

            _userRepository.Create(_user1);

            IEnumerable<Post> newPosts = _postRepository.GetBlogPosts(_nickname);

            Assert.That(newPosts, Is.Not.Null);
            Assert.That(newPosts.Count(), Is.EqualTo(0));
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
        public void
            GivenTheUserHasMultiplePostsForTheSameYearAndMonth_WhenIGetAllPostsForThatYearAndMonth_ThenIGetTheCorrectPosts
            ()
        {
            var posts = new List<Post>
                            {
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2010, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 20), DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 6, 21), DateTime.Today),
                            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname, DateTime.Now)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                .WithBlog(blog);

            var posts2 = new List<Post>
                             {
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2010, 4, 19), DateTime.Today),
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 20), DateTime.Today),
                             };

            Blog blog2 = BuildMeA
                .Blog("title2", "description2", "nickname2", DateTime.Now)
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
        public void
            GivenTheUserHasMultiplePostsForTheSameYearMonthAndDay_WhenIGetAllPostsForThatYearMonthAndDay_ThenIGetTheCorrectPosts
            ()
        {
            var posts = new List<Post>
                            {
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2010, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 6, 21), DateTime.Today),
                            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname, DateTime.Now)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                .WithBlog(blog);

            var posts2 = new List<Post>
                             {
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2010, 4, 19), DateTime.Today),
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 20), DateTime.Today),
                             };

            Blog blog2 = BuildMeA
                .Blog("title2", "description2", "nickname2", DateTime.Now)
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
        public void
            GivenTheUserHasMultiplePostsForTheSameYearMonthDayAndTitle_WhenIGetAllPostsForThatYearMonthDayAndTitle_ThenIGetTheCorrectPosts
            ()
        {
            var posts = new List<Post>
                            {
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2010, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 3", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 4", "entry 1", new DateTime(2011, 6, 21), DateTime.Today),
                            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname, DateTime.Now)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                .WithBlog(blog);

            var posts2 = new List<Post>
                             {
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2010, 4, 19), DateTime.Today),
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 20), DateTime.Today),
                             };

            Blog blog2 = BuildMeA
                .Blog("title2", "description2", "nickname2", DateTime.Now)
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
        public void
            GivenTheUserHasMultiplePostsForTheSameYearMonthDayAndTitle_WhenIGetAllPostsForThatYearMonthDayWithTheWrongTitleTitle_ThenIGetNoPosts
            ()
        {
            var posts = new List<Post>
                            {
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2010, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 3", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 4", "entry 1", new DateTime(2011, 6, 21), DateTime.Today),
                            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname, DateTime.Now)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                .WithBlog(blog);

            var posts2 = new List<Post>
                             {
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2010, 4, 19), DateTime.Today),
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 20), DateTime.Today),
                             };

            Blog blog2 = BuildMeA
                .Blog("title2", "description2", "nickname2", DateTime.Now)
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
        public void GivenTheUserHasMultiplePostsForTheSameYear_WhenIGetAllPostsForThatYear_ThenIGetTheCorrectPosts()
        {
            var posts = new List<Post>
                            {
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2010, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 5, 20), DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 6, 21), DateTime.Today),
                            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname, DateTime.Now)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                .WithBlog(blog);

            var posts2 = new List<Post>
                             {
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2010, 4, 18), DateTime.Today),
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 3, 19), DateTime.Today),
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 20), DateTime.Today),
                             };

            Blog blog2 = BuildMeA
                .Blog("title2", "description2", "nickname2", DateTime.Now)
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
        public void GivenTheUserHasMultiplePosts_WhenIGetAllPostsForThatNickname_ThenIGetTheCorrectPosts()
        {
            var posts = new List<Post>
                            {
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2010, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19), DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 5, 20), DateTime.Today),
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 6, 21), DateTime.Today),
                            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname, DateTime.Now)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                .WithBlog(blog);

            var posts2 = new List<Post>
                             {
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2010, 4, 18), DateTime.Today),
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 3, 19), DateTime.Today),
                                 BuildMeA.Post("title 2", "entry 1", new DateTime(2011, 4, 20), DateTime.Today),
                             };

            Blog blog2 = BuildMeA
                .Blog("title2", "description2", "nickname2", DateTime.Now)
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
        public void GivenThereIsAComment_WhenIRetriveTheOwningPost_ThenCommentIsRetrived()
        {
            Comment comment = BuildMeA.Comment("This is a comment", DateTime.Now);

            var posts = new List<Post>
                            {
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2011, 4, 19), DateTime.Today).
                                    WithComment(comment)
                            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname, DateTime.Now)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                .WithBlog(blog);
            _userRepository.Create(_user1);
            Post newPost = _postRepository.GetBlogPost(posts[0].Id);
            Assert.That(newPost.Comments.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenThereIsAPostWhereCommentsAreDisabled_WhenIAddAComment_TheCommentIGetAnException()
        {
            _userRepository.Create(_user2);
            Assert.Throws<MBlogException>(() => _postRepository.AddComment(_post2.Id, "CommentName", "Comment Text"));
        }

        [Test]
        public void GivenThereIsAPost_WhenIAddAComment_TheCommentIsAdded()
        {
            _userRepository.Create(_user1);
            Post post = _postRepository.AddComment(_post1.Id, "CommentName", "Comment Text");

            Assert.That(post.Comments.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenThereIsNoPost_WhenIAddAComment_ThenAnMBlogExceptionIsThrown()
        {
            _userRepository.Create(_user1);
            Assert.Throws<MBlogException>(() => _postRepository.AddComment(2222, "CommentName", "Comment Text"));
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
        public void WhenIUpdateAPost_AndThePostDoesNotExist_ThenAnExceptionIsThrown()
        {
            var posts = new List<Post>
                            {
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2010, 4, 19), DateTime.Today),
                            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname, DateTime.Now)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                .WithBlog(blog);

            _userRepository.Create(_user1);

            Post post = _postRepository.Entities.Where(p => p.Title == "title 1").FirstOrDefault();
            Assert.Throws<MBlogException>(() => _postRepository.Update(post.Id + 1001, "new title", post.BlogPost));
        }

        [Test]
        public void WhenIUpdateAPost_ThenIPostIsUpdated()
        {
            var posts = new List<Post>
                            {
                                BuildMeA.Post("title 1", "entry 1", new DateTime(2010, 4, 19), DateTime.Today),
                            };

            Blog blog = BuildMeA
                .Blog("title", "description", _nickname, DateTime.Now)
                .WithPosts(posts);

            _user1 = BuildMeA.User("email", "name", "password")
                .WithBlog(blog);

            _userRepository.Create(_user1);

            Post post = _postRepository.Entities.Where(p => p.Title == "title 1").FirstOrDefault();
            _postRepository.Update(post.Id, "new title", post.BlogPost);
            post = _postRepository.Entities.Where(p => p.Title == "new title").FirstOrDefault();

            Assert.That(post, Is.Not.Null);
        }
    }
}