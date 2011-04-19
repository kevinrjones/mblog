using System.Data.Entity;
using MBlogModel;

namespace MBlogRepository.Contexts
{
    public class BlogPostDbContext : DbContext
    {
        public BlogPostDbContext(string connectionString)
            : base(connectionString){}

        public DbSet<Post> Posts { get; set; } 
    }
}