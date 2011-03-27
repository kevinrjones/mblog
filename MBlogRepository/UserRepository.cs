using System.Linq;
using MBlogModel;
using Repository;

namespace MBlogRepository
{
    public class UserRepository : BaseEfRepository<User>, IUserRepository 
    {
        public UserRepository(string connectionString)
            : base(new UserDbContext(connectionString))
        {
        }

        public User GetUser(string email)
        {
            return (from e in Entities
                    where e.Email == email
                    select e).FirstOrDefault();
        }

        public User GetUser(int id)
        {
            return (from e in Entities
                    where e.Id == id
                    select e).FirstOrDefault();
        }
    }
}