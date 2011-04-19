using System.Data.Entity;
using MBlogModel;

namespace MBlogRepository.Contexts
{
    public class UsernameBlacklistDbContext : DbContext
    {
        public UsernameBlacklistDbContext(string connectionString) : base(connectionString) { }

        public DbSet<Blacklist> UsernameBlacklist;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blacklist>().ToTable("username_blacklists");
        }
    }
}