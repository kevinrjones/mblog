using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlogModel;

namespace MBlogRepository.Interfaces
{
    public interface IBlogRepository
    {
        Blog GetBlog(string name);
    }
}
