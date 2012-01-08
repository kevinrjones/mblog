using System.Collections.Generic;
using MBlogDomainInterfaces.ModelState;
using MBlogModel;

namespace MBlogDomainInterfaces
{
    public interface IUserDomain
    {
        User GetUser(string email);
        User GetUser(int id);
        User CreateUser(string name, string email, string password);
        List<ErrorDetails> IsUserRegistrationValid(string name, string email);
        User GetUserWithTheirBlogs(int id);
        IEnumerable<User> GetUsersWithTheirBlogs();
    }
}