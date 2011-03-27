using MBlogModel;
using Repository;

namespace MBlogRepository
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUser(string email);
        User GetUser(int id);
    }
}
