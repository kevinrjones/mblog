using System.Collections.Generic;
using MBlogModel;

namespace MBlogRepository.Interfaces
{
    public interface IPostRepository
    {
        IEnumerable<Post> GetPosts();
    }
}