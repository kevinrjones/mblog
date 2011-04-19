using System.Collections.Generic;
using MBlogModel;
using MBlogRepository.Contexts;
using MBlogRepository.Interfaces;
using Repository;
using System.Linq;

namespace MBlogRepository.Repositories
{
    public class UsernameBlacklistRepository : BaseEfRepository<Blacklist>, IUsernameBlacklistRepository
    {
        public UsernameBlacklistRepository(string connectionString)
            : base(new UsernameBlacklistDbContext(connectionString))
        {
        }

        public List<Blacklist> GetNames()
        {
            return Entities.ToList();
        }

        public Blacklist GetName(string nickname)
        {
            return Entities.Where(e => e.Name == nickname).FirstOrDefault();
        }

    }
}