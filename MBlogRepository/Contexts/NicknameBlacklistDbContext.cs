using System.Data.Entity;
using MBlogModel;

namespace MBlogRepository.Contexts
{
    public class NicknameBlacklistDbContext : DbContext
    {
        public DbSet<Blacklist> NicknameBlacklist;

        public NicknameBlacklistDbContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blacklist>().ToTable("nickname_blacklists");
        }
    }
}