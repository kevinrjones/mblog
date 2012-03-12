using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlogModel;

namespace MBlogDomainInterfaces
{
    public interface IPostDomain
    {
        void AddComment(int postId, string name, string comment);
        Post GetBlogPost(int postId);
        IList<Post> GetBlogPosts();
        IList<Post> GetOrderedBlogPosts(int blogId);
        IList<Post> GetBlogPosts(string nickname);
        IList<Post> GetBlogPosts(int year, int month, int day, string nickname, string link);
        void Delete(int postId);
    }
}
