using MBlogModel;

namespace MBlogDomainInterfaces
{
    public interface IBlogDomain
    {
        Blog GetBlog(string nickname);
        void UpdateBlog(string nickname, bool approveComments, bool commentsEnabled, string description, string title);
        void CreateBlog(string title, string description, bool approveComments, bool commentsEnabled, string nickname, int userId);
    }
}