using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MBlog.ActionResults
{
    public class RssActionResult : ActionResult
    {
        public RssActionResult() { }
        public RssActionResult(SyndicationFeed feed)
        {
            Feed = feed;
        }
        public SyndicationFeed Feed { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            var rssFormatter = new Rss20FeedFormatter(Feed);
            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                rssFormatter.WriteTo(writer);
            }
        }
    }

    public class AtomActionResult : ActionResult
    {
        public SyndicationFeed Feed { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            var rssFormatter = new Atom10FeedFormatter(Feed);
            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                rssFormatter.WriteTo(writer);
            }
        }
    }

    public class SyndicationActionResult : ActionResult
    {
        public SyndicationActionResult() { }
        public SyndicationActionResult(SyndicationFeed feed, Func<SyndicationFeed, FeedData> produceFeedData)
        {
            _produceFeedData = produceFeedData;
            Feed = feed;
        }

        private static SyndicationFeed Feed { get; set; }
        private Func<SyndicationFeed, FeedData> _produceFeedData = delegate { return null; };
        public Func<SyndicationFeed, FeedData> ProduceFeedData
        {
            private get { return _produceFeedData; }
            set { _produceFeedData = value; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            var data = ProduceFeedData(Feed);
            if (data != null)
            {
                response.ContentType = data.ContentType;
                response.AppendHeader("Cache-Control", "private");
                response.AppendHeader("Last-Modified", data.LastModifiedDate.ToString("r"));
                response.AppendHeader("ETag", String.Format("\"{0}\"", data.ETag));
                response.Output.WriteLine(data.Content);
                response.StatusCode = 200;
                response.StatusDescription = "OK";
            }
        }
    }

    public class FeedData
    {
        public string Key { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string ETag { get; set; }
    }

    public static class SyndicationHelper
    {
        public static FeedData GetRssFeed(SyndicationFeed feed)
        {
            var data = new FeedData { ContentType = "application/rss+xml" };
            if (feed.Items.Count() > 0)
            {
                var dat = (from syndicationItem in feed.Items
                           orderby syndicationItem.PublishDate descending
                           select syndicationItem).FirstOrDefault();

                var rssFormatter = new Rss20FeedFormatter(feed);
                StringBuilder feedContent = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(feedContent))
                {
                    rssFormatter.WriteTo(writer);
                }
                data.Content = feedContent.ToString();
                data.LastModifiedDate = dat.PublishDate.DateTime;
                //data.ETag = data.Content.GetHashCode().ToString();
            }
            return data;
        }

        public static FeedData GetAtomFeed(SyndicationFeed feed)
        {
            FeedData data = new FeedData { ContentType = "application/atom+xml" };
            if (feed.Items.Count() > 0)
            {
                var dat = (from syndicationItem in feed.Items
                           orderby syndicationItem.PublishDate descending
                           select syndicationItem).FirstOrDefault();

                var rssFormatter = new Atom10FeedFormatter(feed);
                StringBuilder feedContent = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(feedContent))
                {
                    rssFormatter.WriteTo(writer);
                }
                data.Content = feedContent.ToString();
                data.LastModifiedDate = dat.PublishDate.DateTime;
                //data.ETag = data.Content.GetHashCode().ToString();
            }
            return data;
        }
    }
}