using System;
using System.Linq;
using MBlogModel;
using MBlogRepository.Contexts;
using MBlogRepository.Interfaces;
using Repository;

namespace MBlogRepository.Repositories
{
    public class BlogRepository : BaseEfRepository<Blog>, IBlogRepository
    {
        public BlogRepository(string connectionString)
            : base(new BlogDbContext(connectionString))
        {
        }

        #region IBlogRepository Members

        public Blog GetBlog(string nickname)
        {
            return (from b in Entities
                    where b.Nickname == nickname
                    select b).FirstOrDefault();
        }

        public void UpdateBlogStatistics(int blogId)
        {
            Blog blog = (from b in Entities
                         where b.Id == blogId
                         select b).FirstOrDefault();

            if (blog == null)
            {
                throw new MBlogException("blogId not valid");
            }
            blog.LastUpdated = DateTime.UtcNow;
            blog.TotalPosts++;

            Update(blog);
        }

        #endregion
    }
}