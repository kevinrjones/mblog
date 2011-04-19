using System.Collections.Generic;
using MBlogModel;
using Repository;

namespace MBlogRepository.Interfaces
{
    public interface IBlogPostRepository : IRepository<Post>
    {
        Post GetBlogPost(int id);
        IList<Post> GetBlogPosts(string nickname);
        IList<Post> GetBlogPosts(int year, int month, int day, string nickname, string link);
    }
}
