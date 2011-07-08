using System.Collections.Generic;
using MBlogModel;
using MBlogRepository.Contexts;
using MBlogRepository.Interfaces;
using Repository;
using System.Linq;

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