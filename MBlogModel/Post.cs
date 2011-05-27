using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MBlogModel
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [MaxLength(int.MaxValue)]
        public string BlogPost { get; set; }
        [Required]
        public DateTime Posted { get; set; }
        public DateTime? Edited { get; set; }

        public virtual Blog Blog { get; set; }


        List<Comment> _comments = new List<Comment>();
        public List<Comment> Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public IEnumerable<Comment> ApprovedComments
        {
            get { return Comments.Where(c => c.Approved); }
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
            Edited = DateTime.Now;
        }

        public string TitleLink
        {
            get{return Title.Replace(' ', '-').Replace('/', '-').ToLower(); }
        }
    }
}