using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace MBlog.ActionResults
{
    public static class SyndicationHelper
    {
        private delegate void WriteTo(XmlWriter writer);
        public static FeedData GetRssFeed(this SyndicationFeed feed)
        {
            var rssFormatter = new Rss20FeedFormatter(feed);
            return GetFeed(feed, "application/rss+xml", rssFormatter.WriteTo);
        }

        public static FeedData GetAtomFeed(SyndicationFeed feed)
        {
            var atom10FeedFormatter = new Atom10FeedFormatter(feed);
            return GetFeed(feed, "application/atom+xml", atom10FeedFormatter.WriteTo);
        }

        private static FeedData GetFeed(SyndicationFeed feed, string contentType, WriteTo writeTo)
        {
            var feedData = new FeedData { ContentType = contentType };
            if (feed.Items.Count() > 0)
            {
                var item = (from syndicationItem in feed.Items
                            orderby syndicationItem.PublishDate descending
                            select syndicationItem).FirstOrDefault();

                StringBuilder feedContent = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(feedContent))
                {
                    writeTo(writer);
                }
                feedData.Content = feedContent.ToString();
                feedData.LastModifiedDate = item.PublishDate.DateTime;
                //data.ETag = data.Content.GetHashCode().ToString();
            }
            return feedData;
        }
    }
}