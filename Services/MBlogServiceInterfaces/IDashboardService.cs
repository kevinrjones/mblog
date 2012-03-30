using MBlogModel;

namespace MBlogServiceInterfaces
{
    public interface IDashboardService
    {
        void CreatePost(Post post, int blogId);
        void Update(int postId, string title, string post, int blogId);
    }
}