using System;

namespace MBlog.Models.Comment
{
    public class CommentViewModel
    {
        public CommentViewModel()
        {
            
        }

        public CommentViewModel(MBlogModel.Comment comment)
        {
            this.Comment = comment.CommentText;
            this.Commented = comment.Commented;
            this.EMail = comment.EMail;
            this.Name = comment.Name;
        }

        public string Name { get; set; }
        public string Comment { get; set; }
        public string EMail { get; set; }
        public DateTime Commented { get; set; }

    }
}