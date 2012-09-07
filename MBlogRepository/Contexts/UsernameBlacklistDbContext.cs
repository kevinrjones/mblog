using System.Data.Entity;
using MBlogModel;

namespace MBlogRepository.Contexts
{
    public class UsernameBlacklistDbContext : DbContext
    {
        public DbSet<Blacklist> UsernameBlacklist;

        public UsernameBlacklistDbContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<UsernameBlacklistDbContext>(null);
            modelBuilder.Entity<Blacklist>().ToTable("username_blacklists");
        }
    }
}