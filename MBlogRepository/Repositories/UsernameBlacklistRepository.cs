using MBlogRepository.Contexts;
using MBlogRepository.Interfaces;

namespace MBlogRepository.Repositories
{
    public class UsernameBlacklistRepository : BlacklistRepositoryBase, IUsernameBlacklistRepository
    {
        public UsernameBlacklistRepository(string connectionString)
            : base(new UsernameBlacklistDbContext(connectionString))
        {
        }
    }
}