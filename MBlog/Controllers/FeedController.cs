using System;
using System.Web.Mvc;
using Logging;
using MBlog.ActionResults;
using MBlogDomainInterfaces;

namespace MBlog.Controllers
{
    
    public class FeedController : BaseController
    {
        private readonly ISyndicationFeedDomain _syndicationFeedDomain;

        public FeedController(ISyndicationFeedDomain syndicationFeedDomain, ILogger logger)
            : base(logger, null, null)
        {
            _syndicationFeedDomain = syndicationFeedDomain;           
        }

        [HttpGet]
        public ActionResult Rss(string nickname)
        {
            var feed = _syndicationFeedDomain.CreateSyndicationFeed(nickname, "rss", HttpContext.Request.Url.Scheme, HttpContext.Request.Headers["HOST"]);
            return new SyndicationActionResult(feed.GetRssFeed());
        }

        [HttpGet]
        public ActionResult Atom(string nickname)
        {
            var feed = _syndicationFeedDomain.CreateSyndicationFeed(nickname, "atom", HttpContext.Request.Url.Scheme, HttpContext.Request.Headers["HOST"]);            
            return new SyndicationActionResult(feed.GetAtomFeed());
        }
    }
}
