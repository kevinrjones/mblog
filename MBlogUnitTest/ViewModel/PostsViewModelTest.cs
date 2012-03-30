using System.Collections.Generic;
using MBlog.Models.Post;
using NUnit.Framework;

namespace MBlogUnitTest.ViewModel
{
    [TestFixture]
    public class PostsViewModelTest
    {
        [Test]
        public void GivenAPostsViewModel_WhenIGetACollectionOfPostViewModels_ThenItIsInitialized()
        {
            var model = new PostsViewModel();
            List<PostViewModel> posts = model.Posts;
            Assert.That(posts, Is.Not.Null);
        }
    }
}