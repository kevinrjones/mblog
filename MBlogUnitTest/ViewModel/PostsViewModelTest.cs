using System.Linq;
using System.Text;
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
            PostsViewModel model = new PostsViewModel();
            var posts = model.Posts;
            Assert.That(posts, Is.Not.Null);
        }
    }
}
