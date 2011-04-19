using System.Data.Entity;
using MBlogModel;

namespace MBlogRepository.Contexts
{
    public class NicknameBlacklistDbContext : DbContext
    {
        public NicknameBlacklistDbContext(string connectionString) : base(connectionString) { }

        public DbSet<Blacklist> NicknameBlacklist;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blacklist>().ToTable("nickname_blacklists");
        }
    }
}