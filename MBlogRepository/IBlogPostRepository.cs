using System.Collections.Generic;
using System.Data.Entity;
using MBlogModel;
using Repository;

namespace MBlogRepository
{
    public interface IBlogPostRepository : IRepository<Post>
    {
        Post GetBlogPost(int id);
        IList<Post> GetBlogPosts();
    }
}
