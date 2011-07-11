namespace MBlog.Models.Blog
{
    public class CreateBlogViewModel
    {
        public bool ApproveComments { get; set; }
        public bool IsCreate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool CommentsEnabled { get; set; }
        public string Nickname { get; set; }
    }
}