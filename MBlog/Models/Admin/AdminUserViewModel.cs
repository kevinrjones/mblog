using System.Collections.Generic;

namespace MBlog.Models.Admin
{
    public class AdminUserViewModel
    {
        private List<AdminBlogViewModel> _blogs = new List<AdminBlogViewModel>();
        public int UserId { get; set; }
        public string Name { get; set; }

        public List<AdminBlogViewModel> Blogs
        {
            get { return _blogs; }
            set { _blogs = value; }
        }

        public void AddBlogs(ICollection<MBlogModel.Blog> blogs)
        {
            foreach (MBlogModel.Blog blog in blogs)
            {
                Blogs.Add(new AdminBlogViewModel
                              {
                                  BlogId = blog.Id,
                                  Nickname = blog.Nickname,
                                  Title = blog.Title,
                                  Description = blog.Description
                              });
            }
        }
    }
}