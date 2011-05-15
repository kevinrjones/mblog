using MBlogModel;

namespace MBlogIntegrationTest.Builder
{
    internal class PostBuilder : Builder<Post>
    {
        public PostBuilder()
        {
            Instance = new Post();
        }
    }
}
