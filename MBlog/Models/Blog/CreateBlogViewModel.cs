using System.ComponentModel.DataAnnotations;

namespace MBlog.Models.Blog
{
    public class CreateBlogViewModel
    {
        public CreateBlogViewModel()
        {
        }

        public CreateBlogViewModel(MBlogModel.Blog blog)
        {
            IsCreate = false;
            ApproveComments = blog.ApproveComments;
            Title = blog.Title;
            Description = blog.Description;
            CommentsEnabled = blog.CommentsEnabled;
            Nickname = blog.Nickname;
        }

        public bool CommentsEnabled { get; set; }
        public bool ApproveComments { get; set; }
        public bool IsCreate { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Nickname { get; set; }
    }
}