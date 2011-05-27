using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBlogModel;

namespace MBlog.Models.Admin
{
    public class AdminUserViewModel
    {
        public string Name { get; set; }
        private List<AdminBlogViewModel> _blogs = new List<AdminBlogViewModel>();
        public List<AdminBlogViewModel> Blogs
        {
            get { return _blogs; }
            set { _blogs = value; }
        }
    }
}