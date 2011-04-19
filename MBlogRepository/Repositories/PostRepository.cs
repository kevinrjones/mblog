using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MBlogModel;
using MBlogRepository.Contexts;
using MBlogRepository.Interfaces;
using Repository;

namespace MBlogRepository.Repositories
{
     public class PostRepository : BaseEfRepository<Post>, IPostRepository 
    {
         private const int Count = 10;

         public PostRepository(string connectionString)
            : base(new PostDbContext(connectionString)){}

         public IEnumerable<Post> GetPosts()
         {
            return (from p in Entities.Include("Blog.User")
                       orderby p.Posted descending 
                       select p)
                       .Take(Count)
                       .ToList();
        }
    }
}