using System.Collections.Generic;
using MBlogModel;

namespace MBlogRepository.Interfaces
{
    public interface IPostRepository
    {
        IEnumerable<Post> GetPosts();
        Post GetBlogPost(int id);
        IList<Post> GetBlogPosts(string nickname);
        IList<Post> GetBlogPosts(int year, int month, int day, string nickname, string link);
        Post AddComment(int id, string name, string comment);
    }
}