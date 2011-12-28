using MBlogModel;

namespace MBlogBuilder
{
    public class CommentBuilder : Builder<Comment>
    {
        public CommentBuilder()
        {
            Instance = new Comment();
        }
    }
}