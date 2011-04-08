using System.Collections.Generic;
using MBlogModel;

namespace MBlogRepository
{
    public interface IPostRepository
    {
        IEnumerable<Post> GetPosts();
    }
}