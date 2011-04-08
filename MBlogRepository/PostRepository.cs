using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MBlogModel;
using MBlogUnitTest.Controllers;
using Repository;

namespace MBlogRepository
{
     public class PostRepository : BaseEfRepository<Post>, IPostRepository 
    {
        public PostRepository(string connectionString)
            : base(new PostDbContext(connectionString)){}

         public IEnumerable<Post> GetPosts()
         {
             DbSet<Post> posts = Entities as DbSet<Post>;
            return (from p in Entities.Include("Blog.User")
                       orderby p.Posted descending 
                       select p)
                       .Take(10)
                       .ToList();
        }
    }
}