﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MBlogModel;
using MBlogRepository.Contexts;
using MBlogRepository.Interfaces;
using Repository;

namespace MBlogRepository.Repositories
{
    public class UserRepository : BaseEfRepository<User>, IUserRepository
    {
        public UserRepository(string connectionString)
            : base(new UserDbContext(connectionString))
        {
        }

        #region IUserRepository Members

        public IEnumerable<User> GetUsers()
        {
            return Entities.ToList();
        }

        public IEnumerable<User> GetUsersWithTheirBlogs()
        {
            var users = Entities as DbSet<User>;

            return users.Include("Blogs").ToList();
        }

        public User GetUserWithTheirBlogs(int id)
        {
            var users = Entities as DbSet<User>;

            return (from e in users.Include("Blogs")
                    where e.Id == id
                    select e).FirstOrDefault();
        }

        public User GetUser(string email)
        {
            return (from e in Entities.Include("Blogs")
                    where e.Email == email
                    select e).FirstOrDefault();
        }

        public User GetUser(int id)
        {
            return (from e in Entities
                    where e.Id == id
                    select e).FirstOrDefault();
        }

        #endregion
    }
}