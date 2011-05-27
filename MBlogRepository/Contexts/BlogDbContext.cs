using System.Data.Entity;
using MBlogModel;

namespace MBlogRepository.Contexts
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(string connectionString) : base(connectionString) { }

        public DbSet<Post> Posts { get; set; }
    }
}