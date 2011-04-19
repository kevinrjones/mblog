using System.Collections.Generic;
using MBlogModel;
using Repository;

namespace MBlogRepository.Interfaces
{
    public interface INicknameBlacklistRepository : IRepository<Blacklist>
    {
        List<Blacklist> GetNames();
        Blacklist GetName(string nickname);
    }
}
