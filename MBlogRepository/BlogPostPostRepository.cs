﻿using System;
using System.Collections.Generic;
using System.Linq;
using MBlogModel;
using Repository;

namespace MBlogRepository
{
    public class BlogPostPostRepository : BaseEfRepository<Post>, IBlogPostRepository
    {
        public BlogPostPostRepository(string connectionString) : base(new BlogPostDbContext(connectionString))
        {
        }

        public Post GetBlogPost(int id)
        {
            var b = (from e in Entities
                    where e.Id == id
                    select e).FirstOrDefault();
            return b;
        }

        public IList<Post> GetBlogPosts(string nickname)
        {
            return (from f in Entities
             orderby f.Posted descending 
             where f.Blog.Nickname == nickname
                select f)
                .ToList();                
        }
    }
}