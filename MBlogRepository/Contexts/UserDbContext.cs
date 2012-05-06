using System.Data.Entity;
using MBlogModel;

namespace MBlogRepository.Contexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<UserDbContext>(null);
        }

    }
}