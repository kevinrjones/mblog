using MBlogModel;

namespace MBogIntegrationTest.Builder
{
    internal class PostBuilder : Builder<Post>
    {
        public PostBuilder()
        {
            Instance = new Post();
        }
    }
}
