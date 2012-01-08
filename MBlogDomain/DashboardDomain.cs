using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlogDomainInterfaces;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlogDomain
{
    public class DashboardDomain : IDashboardDomain
    {
        private readonly IPostRepository _postRepository;
        private readonly IBlogRepository _blogRepository;

        public DashboardDomain(IPostRepository postRepository, IBlogRepository blogRepository)
        {
            _postRepository = postRepository;
            _blogRepository = blogRepository;
        }

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
    }
}
