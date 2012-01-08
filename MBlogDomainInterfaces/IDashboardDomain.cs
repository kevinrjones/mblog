using MBlogModel;

namespace MBlogDomainInterfaces
{
    public interface IDashboardDomain
    {
        void CreatePost(Post post, int blogId);
        void Update(int postId, string title, string post, int blogId);
    }
}