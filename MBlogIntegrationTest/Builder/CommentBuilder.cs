using MBlogModel;

namespace MBlogIntegrationTest.Builder
{
    internal class CommentBuilder : Builder<Comment>
    {
        public CommentBuilder()
        {
            Instance = new Comment();
        }
    }
}