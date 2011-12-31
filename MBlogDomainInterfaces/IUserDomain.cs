using System.Collections.Generic;
using MBlogDomainInterfaces.ModelState;
using MBlogModel;

namespace MBlogDomainInterfaces
{
    public interface IUserDomain
    {
        User GetUser(string email);
        User CreateUser(string name, string email, string password);
        List<ErrorDetails> IsUserRegistrationValid(string name, string email);
    }
}