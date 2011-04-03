using System;
using System.ComponentModel.DataAnnotations;

namespace MBlogModel
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; private set; }
        [Required]
        [MaxLength(int.MaxValue)]
        public string BlogPost { get; private set; }
        [Required]
        public DateTime Posted { get; private set; }
        public DateTime? Edited { get; private set; }

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
    }
}