using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MBlogModel;
using MBlogRepository.Contexts;
using MBlogRepository.Interfaces;
using Repository;

namespace MBlogRepository.Repositories
{
    public class PostRepository : BaseEfRepository<Post>, IPostRepository
    {
        private const int Count = 10;

        public PostRepository(string connectionString)
            : base(new PostDbContext(connectionString)) { }

        public IEnumerable<Post> GetPosts()
        {
            return GetPostsAndComments().ToList();
        }

        public Post GetBlogPost(int id)
        {
            var b = (from p in Entities.Include(p => p.Comments)
                         .Include(p=>p.Blog.User)
                     where p.Id == id
                     select p).FirstOrDefault();
            return b;
        }

        public IList<Post> GetBlogPosts(string nickname)
        {
            if (string.IsNullOrEmpty(nickname))
            {
                return (IList<Post>)GetPosts();
            }
            return SelectAllForNickname(nickname);
        }

        public IList<Post> GetBlogPosts(int blogId)
        {            
            return (from e in Entities
                    where e.BlogId == blogId
                    select e).ToList();
        }

        public Post AddComment(int id, string name, string comment)
        {
            Post post = GetBlogPost(id);
            if (!post.CommentsEnabled)
            {
                throw new MBlogException("Comments are disabled for this post");
            }
            post.Comments.Add(new Comment { Name = name, CommentText = comment, Commented = DateTime.UtcNow, Approved = !post.Blog.ApproveComments});
            Save();
            return post;
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
            return SelectByTitle(year, nickname, month, day, link);
        }

        private IList<Post> SelectByTitle(int year, string nickname, int month, int day, string link)
        {
            var posts = GetPostsAndComments().Where(post => post.Blog.Nickname == nickname
                                                            && post.Posted.Year == year
                                                            && post.Posted.Month == month
                                                            && post.Posted.Day == day).ToList();

            return (from post in posts
                    where post.TitleLink == link
                    select post).ToList();
        }

        private IList<Post> SelectAllForNicknameAndYearAndMonthAndDay(int year, string nickname, int month, int day)
        {
            return (GetPostsAndComments().Where(post => post.Blog.Nickname == nickname
                                                        && post.Posted.Year == year
                                                        && post.Posted.Month == month
                                                        && post.Posted.Day == day))
                .ToList();
        }

        private IList<Post> SelectAllForNicknameAndYearAndMonth(int year, string nickname, int month)
        {
            return (GetPostsAndComments().Where(post => post.Blog.Nickname == nickname
                                                        && post.Posted.Year == year
                                                        && post.Posted.Month == month))
                .ToList();
        }

        private IList<Post> SelectAllForNicknameAndYear(int year, string nickname)
        {
            return (GetPostsAndComments().Where(post => post.Blog.Nickname == nickname
                                                       && post.Posted.Year == year))
                .ToList();
        }

        private IList<Post> SelectAllForNickname(string nickname)
        {
            return (GetPostsAndComments().Where(
                post => post.Blog.Nickname == nickname))
                .ToList();
        }

        private IQueryable<Post> GetPostsAndComments()
        {
            return Entities
                .Include(p => p.Comments)
                .Include(p => p.Blog.User)
                .OrderByDescending(post => post.Posted)
                .Take(Count);
        }
    }
}