using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBlog.Models.Admin
{
    public class AdminBlogViewModel
    {
        public string Nickname { get; set; }
        private string _title;

        public string Title
        {
            get
            {
                if (_title.Length < 60)
                {
                    return _title;
                }
                return _title.Substring(0, 60) + "...";
            }
            set { _title = value; }
        }

        public string Description { get; set; }

        public int BlogId { get; set; }
    }
}