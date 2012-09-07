using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBlog.Models.Feed
{
    public class NewPostViewModel
    {
        public NewPostViewModel(MBlogModel.Post post)       
        {
            DatePosted = post.Posted;
            Id = post.Id;
            Title = post.Title;
            Post = post.BlogPost;
            BlogId = post.BlogId;
        }

        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Post { get; set; }
        public DateTime DatePosted { get; set; }
        public int Id { get; set; }
    }
}