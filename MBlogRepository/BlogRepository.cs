using System;
using System.Collections.Generic;
using System.Linq;
using MBlogModel;
using Repository;

namespace MBlogRepository
{
    public class BlogRepository : BaseEfRepository<Post>, IBlogRepository
    {
        public BlogRepository(string connectionString) : base(new BlogDbContext(connectionString))
        {
        }

        public Post GetBlog(int id)
        {
            var b = (from e in Entities
                    where e.Id == id
                    select e).FirstOrDefault();
            return b;
        }

        public IList<Post> GetBlogs()
        {
            return 
            (from f in Entities
                select f)
                .ToList();                
        }
    }
}