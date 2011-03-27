using System.Data.Entity;

namespace MBlogModel
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(string connectionString)
            : base(connectionString)
        { }
        public DbSet<User> Users { get; set; }
    }
}
