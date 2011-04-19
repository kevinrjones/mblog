using System.Collections.Generic;
using System.Linq;
using MBlogModel;
using MBlogRepository.Contexts;
using MBlogRepository.Interfaces;
using Repository;

namespace MBlogRepository.Repositories
{
    public class BlogPostRepository : BaseEfRepository<Post>, IBlogPostRepository
    {
        public BlogPostRepository(string connectionString)
            : base(new BlogPostDbContext(connectionString))
        {
        }

        public Post GetBlogPost(int id)
        {
            var b = (from e in Entities
                     where e.Id == id
                     select e).FirstOrDefault();
            return b;
        }

        public IList<Post> GetBlogPosts(string nickname)
        {

            if (string.IsNullOrEmpty(nickname))
                return (from f in Entities
                        orderby f.Posted descending
                        select f)
                    .ToList();

            return (from f in Entities
                    orderby f.Posted descending
                    where f.Blog.Nickname == nickname
                    select f)
                .ToList();
        }

        public IList<Post> GetBlogPosts(int year, int month, int day, string nickname, string link)
        {
            if (year == 0)
            {
                return SelectAllForNickname(nickname);                
            }
            if (month == 0)
            {
                return SelectAllForNicknameAndYear(year, nickname);
            }
            if (day == 0)
            {
                return SelectAllForNicknameAndYearAndMonth(year, nickname, month);
            }
            if (string.IsNullOrEmpty(link))
            {
                return SelectAllForNicknameAndYearAndMonthAndDay(year, nickname, month, day);
            }
            // add link filter
            return (from post in Entities
                    orderby post.Posted descending
                    where post.Blog.Nickname == nickname
                          && post.Posted.Year == year
                          && post.Posted.Month == month
                          && post.Posted.Day == day
                    select post)
                .ToList();
        }

        private IList<Post> SelectAllForNicknameAndYearAndMonthAndDay(int year, string nickname, int month, int day)
        {
            return (from post in Entities
                    orderby post.Posted descending
                    where post.Blog.Nickname == nickname
                          && post.Posted.Year == year
                          && post.Posted.Month == month
                          && post.Posted.Day == day
                    select post)
                .ToList();
        }

        private IList<Post> SelectAllForNicknameAndYearAndMonth(int year, string nickname, int month)
        {
            return (from post in Entities
                    orderby post.Posted descending
                    where post.Blog.Nickname == nickname
                          && post.Posted.Year == year
                          && post.Posted.Month == month
                    select post)
                .ToList();
        }

        private IList<Post> SelectAllForNicknameAndYear(int year, string nickname)
        {
            return (from post in Entities
                    orderby post.Posted descending
                    where post.Blog.Nickname == nickname
                          && post.Posted.Year == year
                    select post)
                .ToList();
        }

        private IList<Post> SelectAllForNickname(string nickname)
        {
            return (from post in Entities
                    orderby post.Posted descending
                    where post.Blog.Nickname == nickname
                    select post)
                .ToList();
        }
    }
}