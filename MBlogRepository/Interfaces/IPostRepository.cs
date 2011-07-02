using System.Collections.Generic;
using MBlogModel;
using MBlogRepository.Repositories;
using Repository;

namespace MBlogRepository.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        IEnumerable<Post> GetPosts();
        Post GetBlogPost(int id);
        IList<Post> GetBlogPosts(string nickname);
        IList<Post> GetBlogPosts(int blogId);
        IList<Post> GetBlogPosts(int year, int month, int day, string nickname, string link);
        Post AddComment(int id, string name, string comment);
    }
}