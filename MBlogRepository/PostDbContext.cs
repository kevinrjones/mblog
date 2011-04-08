using System.Data.Entity;
using MBlogModel;

namespace MBlogUnitTest.Controllers
{
    public class PostDbContext : DbContext
    {
        public PostDbContext(string connectionString) : base(connectionString) { }

        public DbSet<Post> Posts { get; set; }
    }
}