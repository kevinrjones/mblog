using System;
using System.ComponentModel.DataAnnotations;

namespace MBlogModel
{
    public class Comment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }

        [Required]
        public DateTime Commented { get; set; }

        [Required, Column("Text")]
        public string CommentText { get; set; }

        [Column("post_id")]
        public int PostId { get; set; }
    }
}