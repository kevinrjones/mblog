using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlogModel;
using Repository;

namespace MBlogRepository.Interfaces
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Blog GetBlog(string name);
    }
}
