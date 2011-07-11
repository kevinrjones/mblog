using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MBlogModel
{
    public class Post
    {
        public virtual int Id { get; set; }
        [Required]
        public virtual string Title { get; set; }
        [Required]
        [MaxLength(int.MaxValue)]
        public virtual string BlogPost { get; set; }
        [Required]
        public virtual DateTime Posted { get; set; }
        public virtual DateTime? Edited { get; set; }

        [Column("blog_id")]
        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }

        List<Comment> _comments = new List<Comment>();
        public virtual List<Comment> Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public void AddPost(string title, string entry)
        {
            Title = title;
            BlogPost = entry;
            Posted = DateTime.Now;
        }

        public void UpdatePost(string title, string entry)
        {
            if(!string.IsNullOrEmpty(title))
                Title = title;
            if (!string.IsNullOrEmpty(entry))
                BlogPost = entry;
            Edited = DateTime.UtcNow;

        }

        public string TitleLink
        {
            get{return Title == null ? "" : Title.Replace(' ', '-').Replace('/', '-').ToLower(); }
        }

        [Column("comments_enabled")]
        public bool CommentsEnabled { get; set; }
    }
}