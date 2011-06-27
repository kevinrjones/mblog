using System;
using CodeKicker.BBCode;

namespace MBlog.Models.Comment
{
    public class CommentViewModel
    {
        public CommentViewModel()
        {
            
        }

        public CommentViewModel(MBlogModel.Comment comment)
        {
            Comment = BBCode.ToHtml(comment.CommentText);
            Commented = comment.Commented;
            EMail = comment.EMail;
            Name = comment.Name ?? "Anonymous";
        }

        public string Name { get; set; }
        public string Comment { get; set; }
        public string EMail { get; set; }
        public DateTime Commented { get; set; }

    }
}