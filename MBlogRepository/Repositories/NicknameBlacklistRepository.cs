using MBlogRepository.Contexts;
using MBlogRepository.Interfaces;

namespace MBlogRepository.Repositories
{
    public class NicknameBlacklistRepository : BlacklistRepositoryBase, INicknameBlacklistRepository
    {
        public NicknameBlacklistRepository(string connectionString)
            : base(new NicknameBlacklistDbContext(connectionString))
        {
        }
    }
}