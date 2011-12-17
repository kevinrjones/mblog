﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Logging;
using MBlog.ActionResults;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    
    public class FeedController : BaseController
    {
        private readonly IPostRepository _postRepository;

        public FeedController(IBlogRepository blogRepository, IPostRepository postRepository,
                              IUserRepository userRepository, ILogger logger)
            : base(logger, userRepository, blogRepository)
        {
            _postRepository = postRepository;
        }


        [HttpGet]
        public ActionResult Rss(string nickname)
        {
            var feed = CreateSyndicationFeed(nickname, "rss");

            return new SyndicationActionResult(feed, SyndicationHelper.GetRssFeed);
        }

        [HttpGet]
        public ActionResult Atom(string nickname)
        {
            var feed = CreateSyndicationFeed(nickname, "atom");
            return new SyndicationActionResult(feed, SyndicationHelper.GetAtomFeed);
        }

        private SyndicationFeed CreateSyndicationFeed(string nickname, string feedType)
        {
            var posts = _postRepository.GetBlogPosts(nickname);
            var blog = BlogRepository.GetBlog(nickname);
            
            string url = string.Format("{0}://{1}/{2}", HttpContext.Request.Url.Scheme, HttpContext.Request.Headers["HOST"], nickname);
            var feed = new SyndicationFeed(blog.Title, blog.Description, new Uri(url), url, blog.LastUpdated);
            feed.Authors.Add(new SyndicationPerson { Name = blog.User.Name });
            feed.Links.Add(SyndicationLink.CreateSelfLink(new Uri(url + "/feed/" + feedType)));

            var items = new List<SyndicationItem>();
            foreach (var post in posts)
            {               
                url = string.Format("{0}://{1}/{2}/{3}/{4}/{5}/{6}", HttpContext.Request.Url.Scheme, HttpContext.Request.Headers["HOST"], nickname, post.Posted.Year, post.Posted.Month, post.Posted.Day, post.TitleLink);

                var item = new SyndicationItem();
                item.Title = new TextSyndicationContent(post.Title, TextSyndicationContentKind.Html);
                item.Content = new TextSyndicationContent(post.BlogPost, TextSyndicationContentKind.Html);
                item.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(url)));
                item.PublishDate = post.Edited;
                item.Id = url;                
                items.Add(item);
            }
            feed.Items = items;
            return feed;
        }
    }
}
