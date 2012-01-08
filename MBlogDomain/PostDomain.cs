using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlogDomainInterfaces;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlogDomain
{
    public class PostDomain : IPostDomain
    {
        private readonly IPostRepository _postRepository;

        public PostDomain(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public void AddComment(int postId, string name, string comment)
        {
            try
            {
                _postRepository.AddComment(postId, name, comment);
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to add comment", e);
            }
        }

        public IList<Post> GetOrderedBlogPosts(int blogId)
        {
            try
            {
                return _postRepository.GetOrderedBlogPosts(blogId);
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to retrieve posts", e);
            }
        }

        public IList<Post> GetBlogPosts(string nickname)
        {
            try
            {
                return _postRepository.GetBlogPosts(nickname);
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to retrieve posts", e);
            }
        }

        public IList<Post> GetBlogPosts()
        {
            try
            {
                return _postRepository.GetPosts().ToList();
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to retrieve posts", e);
            }
        }

        public Post GetBlogPost(int postId)
        {
            try
            {
                return _postRepository.GetBlogPost(postId);
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to retrieve post", e);
            }
        }

        public IList<Post> GetBlogPosts(int year, int month, int day, string nickname, string link)
        {
            try
            {
                return _postRepository.GetBlogPosts(year, month, day, nickname, link);
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to retrieve posts", e);
            }
        }
    }
}
