using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MBlogModel;
using Repository;

namespace MBlogRepository.Repositories
{
    public class BlacklistRepositoryBase: BaseEfRepository<Blacklist>
    {
        public BlacklistRepositoryBase(DbContext dbContext)
            : base(dbContext){}

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