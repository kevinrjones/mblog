using System;
using MBlogModel;
using MBlogRepository.Interfaces;
using MBlogServiceInterfaces;

namespace MBlogService
{
    public class DashboardService : IDashboardService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IPostRepository _postRepository;

        public DashboardService(IPostRepository postRepository, IBlogRepository blogRepository)
        {
            _postRepository = postRepository;
            _blogRepository = blogRepository;
        }

        #region IDashboardService Members

        public void CreatePost(Post post, int blogId)
        {
            try
            {
                _postRepository.Create(post);
                _blogRepository.ChangeBlogLastupdateDate(blogId);
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to create post", e);
            }
        }

        public void Update(int postId, string title, string post, int blogId)
        {
            try
            {
                _postRepository.Update(postId, title, post);
                _blogRepository.ChangeBlogLastupdateDate(blogId);
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to update post", e);
            }
        }

        #endregion
    }
}