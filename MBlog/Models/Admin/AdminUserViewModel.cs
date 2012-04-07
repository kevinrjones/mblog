using System.Collections.Generic;

namespace MBlog.Models.Admin
{
    public class AdminUserViewModel
    {
        private List<AdminBlogViewModel> _blogs = new List<AdminBlogViewModel>();

        public AdminUserViewModel()
        {
        }


        public AdminUserViewModel(string name, int userId, ICollection<MBlogModel.Blog> blogs) : this()
        {
            Name = name;
            UserId = userId;
            AddBlogs(blogs);
        }

        public int UserId { get; set; }
        public string Name { get; set; }


        public List<AdminBlogViewModel> Blogs
        {
            get { return _blogs; }
            set { _blogs = value; }
        }

        private void AddBlogs(ICollection<MBlogModel.Blog> blogs)
        {
            foreach (MBlogModel.Blog blog in blogs)
            {
                Blogs.Add(new AdminBlogViewModel
                              {
                                  BlogId = blog.Id,
                                  Nickname = blog.Nickname,
                                  Title = blog.Title,
                                  Description = blog.Description,
                                  LastUpdated = blog.LastUpdated,
                                  NumberOfPosts = blog.TotalPosts
                              });
            }
        }
    }
}