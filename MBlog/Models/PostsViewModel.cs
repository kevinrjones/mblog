using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBlog.Models
{
    public class PostsViewModel
    {
        private List<PostViewModel> _posts = new List<PostViewModel>();
        
        public List<PostViewModel> Posts
        {
            get { return _posts; }
            set { _posts = value; }
        }

        public bool ShowComments { get; set; }
    }
}