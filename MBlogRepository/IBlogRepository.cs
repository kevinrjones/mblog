using System.Collections.Generic;
using System.Data.Entity;
using MBlogModel;
using Repository;

namespace MBlogRepository
{
    public interface IBlogRepository : IRepository<Post>
    {
        Post GetBlog(int id);
        IList<Post> GetBlogs();
    }
}
