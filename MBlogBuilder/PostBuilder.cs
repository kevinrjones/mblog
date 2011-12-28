using MBlogModel;

namespace MBlogBuilder
{
    public class PostBuilder : Builder<Post>
    {
        public PostBuilder()
        {
            Instance = new Post();
        }

        public PostBuilder WithComment(Comment comment)
        {
            if (comment != null)
            {
                Instance.Comments.Add(comment);
            }
            return this;
        }
    }
}