using System;
using System.Collections.Generic;
using System.Linq;
using MBlogModel;
using MBlogRepository.Interfaces;
using MBlogServiceInterfaces;

namespace MBlogService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        #region IPostService Members

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

        public void Delete(int postId)
        {
            try
            {
                Post post = _postRepository.GetBlogPost(postId);
                _postRepository.Delete(post);
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to retrieve posts", e);
            }
        }

        #endregion
    }
}