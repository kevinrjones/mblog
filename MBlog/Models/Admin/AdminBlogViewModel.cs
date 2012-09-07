using System;

namespace MBlog.Models.Admin
{
    public class AdminBlogViewModel
    {
        private string _title;
        public string Nickname { get; set; }
        public DateTime LastUpdated { get; set; }
        public int NumberOfPosts { get; set; }

        public string Title
        {
            get
            {
                if (_title.Length < 60)
                {
                    return _title;
                }
                return _title.Substring(0, 57) + "...";
            }
            set { _title = value; }
        }

        public string Description { get; set; }
    }
}