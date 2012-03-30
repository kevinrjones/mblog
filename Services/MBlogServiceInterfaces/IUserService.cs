using System.Collections.Generic;
using MBlogModel;
using MBlogServiceInterfaces.ModelState;

namespace MBlogServiceInterfaces
{
    public interface IUserService
    {
        User GetUser(string email);
        User GetUser(int id);
        User CreateUser(string name, string email, string password);
        List<ErrorDetails> IsUserRegistrationValid(string name, string email);
        User GetUserWithTheirBlogs(int id);
        IEnumerable<User> GetUsersWithTheirBlogs();
    }
}