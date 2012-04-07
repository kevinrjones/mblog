using System;
using MBlogModel;
using MBlogRepository.Interfaces;
using MBlogServiceInterfaces;

namespace MBlogService
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        #region IBlogService Members

        public Blog GetBlog(string nickname)
        {
            try
            {
                return _blogRepository.GetBlog(nickname);
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to retireve blog", e);
            }
        }

        public void UpdateBlog(string nickname, bool approveComments, bool commentsEnabled, string description,
                               string title)
        {
            try
            {
                Blog blog = _blogRepository.GetBlog(nickname);

                blog.ApproveComments = approveComments;
                blog.CommentsEnabled = commentsEnabled;
                blog.Description = description;
                blog.Title = title;
                _blogRepository.Update(blog);
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to update blog", e);
            }
        }

        public void CreateBlog(string title, string description, bool approveComments, bool commentsEnabled,
                               string nickname, int userId)
        {
            try
            {
                var blog = new Blog(title, description, approveComments, commentsEnabled, nickname, userId);
                _blogRepository.Create(blog);
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to create blog", e);
            }
        }

        #endregion
    }
}