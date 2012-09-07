using System.ServiceModel.Syndication;
using System.Web.Mvc;
using Logging;
using MBlog.ActionResults;
using MBlogServiceInterfaces;

namespace MBlog.Controllers
{
    public partial class FeedController : BaseController
    {
        private readonly ISyndicationFeedService _syndicationFeedService;

        public FeedController(ISyndicationFeedService syndicationFeedService, ILogger logger)
            : base(logger)
        {
            _syndicationFeedService = syndicationFeedService;
        }

        [HttpGet]
        public virtual ActionResult Rss(string nickname)
        {
            SyndicationFeed feed = _syndicationFeedService.CreateSyndicationFeed(nickname, "rss",
                                                                                 HttpContext.Request.Url.Scheme,
                                                                                 HttpContext.Request.Headers["HOST"]);
            return new SyndicationActionResult(feed.GetRssFeed());
        }

        [HttpGet]
        public virtual ActionResult Atom(string nickname)
        {
            SyndicationFeed feed = _syndicationFeedService.CreateSyndicationFeed(nickname, "atom",
                                                                                 HttpContext.Request.Url.Scheme,
                                                                                 HttpContext.Request.Headers["HOST"]);
            return new SyndicationActionResult(feed.GetAtomFeed());
        }
    }
}