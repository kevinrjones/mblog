using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlogDomainInterfaces;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlogDomain
{
    public class BlogDomain : IBlogDomain
    {
        private readonly IBlogRepository _blogRepository;

        public BlogDomain(IBlogRepository blogRepository, string connectionString)
        {
            
        }
        public BlogDomain(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

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

        public void UpdateBlog(string nickname, bool approveComments, bool commentsEnabled, string description, string title)
        {
            try
            {
                Blog blog = _blogRepository.GetBlog(nickname);

                blog.ApproveComments = approveComments;
                blog.CommentsEnabled = commentsEnabled;
                blog.Description = description;
                blog.Title = title;
                _blogRepository.Attach(blog);
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to update blog", e);
            }
        }

        public void CreateBlog(string title, string description, bool approveComments, bool commentsEnabled, string nickname, int userId)
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
    }
}
