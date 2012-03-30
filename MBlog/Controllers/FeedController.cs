using System;
using System.Web.Mvc;
using Logging;
using MBlog.ActionResults;
using MBlogServiceInterfaces;

namespace MBlog.Controllers
{
    
    public class FeedController : BaseController
    {
        private readonly ISyndicationFeedService _syndicationFeedService;

        public FeedController(ISyndicationFeedService syndicationFeedService, ILogger logger)
            : base(logger)
        {
            _syndicationFeedService = syndicationFeedService;           
        }

        [HttpGet]
        public ActionResult Rss(string nickname)
        {
            var feed = _syndicationFeedService.CreateSyndicationFeed(nickname, "rss", HttpContext.Request.Url.Scheme, HttpContext.Request.Headers["HOST"]);
            return new SyndicationActionResult(feed.GetRssFeed());
        }

        [HttpGet]
        public ActionResult Atom(string nickname)
        {
            var feed = _syndicationFeedService.CreateSyndicationFeed(nickname, "atom", HttpContext.Request.Url.Scheme, HttpContext.Request.Headers["HOST"]);            
            return new SyndicationActionResult(feed.GetAtomFeed());
        }
    }
}
