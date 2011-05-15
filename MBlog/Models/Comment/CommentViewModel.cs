using System;

namespace MBlog.Models.Comment
{
    public class CommentViewModel
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public string EMail { get; set; }
        public DateTime Commented { get; set; }

    }
}