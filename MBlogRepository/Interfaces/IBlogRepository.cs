﻿using MBlogModel;
using Repository;

namespace MBlogRepository.Interfaces
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Blog GetBlog(string nickname);
        void UpdateBlogStatistics(int blogId);
    }
}