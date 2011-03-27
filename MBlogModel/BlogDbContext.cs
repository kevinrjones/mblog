using System.Data.Entity;

namespace MBlogModel
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(string connectionString)
            : base(connectionString)
        {}
        public DbSet<Post> Posts{ get; set; }        
    }
}