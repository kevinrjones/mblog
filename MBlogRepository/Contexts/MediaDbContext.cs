using System.Data.Entity;
using MBlogModel;

namespace MBlogRepository.Contexts
{
    public class MediaDbContext : DbContext
    {
        public MediaDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<Media> Users { get; set; }
    }
}