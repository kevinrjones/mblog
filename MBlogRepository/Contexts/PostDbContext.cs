using System.Data.Entity;
using MBlogModel;

namespace MBlogRepository.Contexts
{
    public class PostDbContext : DbContext
    {
        public PostDbContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<PostDbContext>(null);
        }

    }
}