using System.Collections.Generic;
using MBlogModel;
using Repository;

namespace MBlogRepository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetUsers();
        IEnumerable<User> GetUsersWithTheirBlogs();
        User GetUser(string email);
        User GetUser(int id);
        User GetUserWithTheirBlogs(int id);
    }
}
