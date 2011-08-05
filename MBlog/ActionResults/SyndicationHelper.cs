using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace MBlog.ActionResults
{
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