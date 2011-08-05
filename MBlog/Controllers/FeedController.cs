using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MBlog.ActionResults;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class FeedController : BaseController
    {
        private readonly IPostRepository _postRepository;

        public FeedController(IBlogRepository blogRepository, IPostRepository postRepository,
                              IUserRepository userRepository)
            : base(userRepository, blogRepository)
        {
            _postRepository = postRepository;
        }


        [HttpGet]
        public ActionResult Rss(string nickname)
        {
            var feed = CreateSyndicationFeed(nickname);

            return new SyndicationActionResult(feed, SyndicationHelper.GetRssFeed);
        }

        [HttpGet]
        public ActionResult Atom(string nickname)
        {
            var feed = CreateSyndicationFeed(nickname);
            return new SyndicationActionResult(feed, SyndicationHelper.GetAtomFeed);
        }

        private SyndicationFeed CreateSyndicationFeed(string nickname)
        {
            var posts = _postRepository.GetBlogPosts(nickname);
            var blog = BlogRepository.GetBlog(nickname);
            string url = Url.Action("Index", "Post", new {nickname}, "http");
            var feed = new SyndicationFeed(blog.Title,
                                           blog.Description,
                                           new Uri(url),
                                           blog.Nickname,
                                           blog.LastUpdated);

            var items = new List<SyndicationItem>();
            foreach (var post in posts)
            {
                //url = Url.Action("Show", "Post", new {Nickname = nickname, year=post.Posted.Year, month=post.Posted.Month, day=post.Posted.Day, link=post.TitleLink}, "http");

                url = string.Format("{0}://{1}/{2}/{3}/{4}/{5}/{6}", HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Authority, nickname, post.Posted.Year, post.Posted.Month, post.Posted.Day, post.TitleLink);

                var item = new SyndicationItem(post.Title,
                                               post.BlogPost,
                                               new Uri(url),
                                               post.Id.ToString(),
                                               post.Edited);
                items.Add(item);
            }
            feed.Items = items;
            return feed;
        }
    }
}
