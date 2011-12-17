using System.Data.Entity;
using MBlogModel;

namespace MBlogRepository.Contexts
{
    public class ImageDbContext : DbContext
    {
        public ImageDbContext(string connectionString)
            : base(connectionString)
        { }

        public DbSet<Image> Users { get; set; } 

    }
}