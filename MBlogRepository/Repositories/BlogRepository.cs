using System.Data.Entity;
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

        public Blog GetBlog(string name)
        {
            return (from b in Entities
                   where b.Nickname == name
                   select b).FirstOrDefault();
        }

        //public IList<Post> GetBlogWithPosts(int year, int month, int day, string nickname, string link)
        //{
        //    if (year == 0)
        //    {
        //        return SelectAllForNickname(nickname);
        //    }
        //    if (month == 0)
        //    {
        //        return SelectAllForNicknameAndYear(year, nickname);
        //    }
        //    if (day == 0)
        //    {
        //        return SelectAllForNicknameAndYearAndMonth(year, nickname, month);
        //    }
        //    if (string.IsNullOrEmpty(link))
        //    {
        //        return SelectAllForNicknameAndYearAndMonthAndDay(year, nickname, month, day);
        //    }
        //    // add link filter
        //    return SeelectByTitle(year, nickname, month, day, link);
        //}

        //private Blog SeelectByTitle(int year, string nickname, int month, int day, string link)
        //{
        //    var blog = from b in Entities.Include("Posts").Include("Posts.Comments")
        //               where (b.Nickname == nickname
        //               && b.Posts.Where(post => post.Posted.Year== year
        //                                        && post.Posted.Month == month
        //                                        && post.Posted.Day == day))

        //    //var blog = GetPostsAndComments().Where(blog => blog.Nickname == nickname
        //    //                                                && post.Posted.Year == year
        //    //                                                && post.Posted.Month == month
        //    //                                                && post.Posted.Day == day).ToList();

        //    //return (from post in posts
        //    //        where post.ToTitleLink() == link
        //    //        select post).ToList();
        //}

        //private IList<Post> SelectAllForNicknameAndYearAndMonthAndDay(int year, string nickname, int month, int day)
        //{
        //    return (GetPostsAndComments().Where(post => post.Blog.Nickname == nickname
        //                                                && post.Posted.Year == year
        //                                                && post.Posted.Month == month
        //                                                && post.Posted.Day == day))
        //        .ToList();
        //}

        //private IList<Post> SelectAllForNicknameAndYearAndMonth(int year, string nickname, int month)
        //{
        //    return (GetPostsAndComments().Where(post => post.Blog.Nickname == nickname
        //                                                && post.Posted.Year == year
        //                                                && post.Posted.Month == month))
        //        .ToList();
        //}

        //private IList<Post> SelectAllForNicknameAndYear(int year, string nickname)
        //{
        //    return (GetPostsAndComments().Where(post => post.Blog.Nickname == nickname
        //                                               && post.Posted.Year == year))
        //        .ToList();
        //}

        //private IList<Post> SelectAllForNickname(string nickname)
        //{
        //    return (GetPostsAndComments().Where(
        //        post => post.Blog.Nickname == nickname))
        //        .ToList();
        //}

        //private IOrderedQueryable<Post> GetPostsAndComments()
        //{
        //    return Entities.Include("Comments").OrderByDescending(post => post Posted);
        //}

    }
}
