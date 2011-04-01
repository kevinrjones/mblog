using System.Data.Entity;
using MBlogModel;

namespace MBlogRepository
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(string connectionString)
            : base(connectionString)
        { }
        public DbSet<User> Users { get; set; }
    }
}
