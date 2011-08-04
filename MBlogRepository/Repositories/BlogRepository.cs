﻿using System.Data.Entity;
using System.Linq;
using MBlogModel;
using MBlogRepository.Contexts;
using MBlogRepository.Interfaces;
using Repository;

namespace MBlogRepository.Repositories
{
    public class BlogRepository : BaseEfRepository<Blog>, IBlogRepository
    {
        public BlogRepository(string connectionString)
            : base(new BlogDbContext(connectionString))
        {
        }

        public Blog GetBlog(string nickname)
        {
            return (from b in Entities
                   where b.Nickname == nickname
                   select b).FirstOrDefault();
        }
    }
}
