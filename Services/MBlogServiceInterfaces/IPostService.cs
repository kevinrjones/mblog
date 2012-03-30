using System.Collections.Generic;
using MBlogModel;

namespace MBlogServiceInterfaces
{
    public interface IPostService
    {
        void AddComment(int postId, string name, string comment);
        Post GetBlogPost(int postId);
        IList<Post> GetBlogPosts();
        IList<Post> GetOrderedBlogPosts(int blogId);
        IList<Post> GetBlogPosts(string nickname);
        IList<Post> GetBlogPosts(int year, int month, int day, string nickname, string link);
        void Delete(int postId);
    }
}