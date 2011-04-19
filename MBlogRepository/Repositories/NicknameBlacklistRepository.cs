using System.Collections.Generic;
using System.Linq;
using MBlogModel;
using MBlogRepository.Contexts;
using MBlogRepository.Interfaces;
using Repository;

namespace MBlogRepository.Repositories
{
    public class NicknameBlacklistRepository : BaseEfRepository<Blacklist>, INicknameBlacklistRepository
    {
        public NicknameBlacklistRepository(string connectionString)
            : base(new NicknameBlacklistDbContext(connectionString))
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