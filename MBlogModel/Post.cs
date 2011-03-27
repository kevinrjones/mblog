using System;
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
    }
}