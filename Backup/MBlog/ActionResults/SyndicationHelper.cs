using System.Collections.ObjectModel;
using System.IO;
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

        public static FeedData GetAtomFeed(this SyndicationFeed feed)
        {
            var atom10FeedFormatter = new Atom10FeedFormatter(feed);
            return GetFeed(feed, "application/atom+xml", atom10FeedFormatter.WriteTo);
        }

        private static FeedData GetFeed(SyndicationFeed feed, string contentType, WriteTo writeTo)
        {
            var feedData = new FeedData { ContentType = contentType };
            if (feed.Items.Any())
            {
                var item = (from syndicationItem in feed.Items
                            orderby syndicationItem.PublishDate descending
                            select syndicationItem).FirstOrDefault();                               
                
                var xmlWriterSettings = new XmlWriterSettings { Encoding = new UTF8Encoding(false) };

                var memoryStream = new MemoryStream();
                
                using (XmlWriter writer = XmlWriter.Create(memoryStream, xmlWriterSettings))
                {
                    writeTo(writer);
                }

                memoryStream.Position = 0;
                var sr = new StreamReader(memoryStream);
                feedData.Content = sr.ReadToEnd();
                feedData.LastModifiedDate = item.PublishDate.DateTime;
                feedData.ETag = feedData.Content.GetHashCode().ToString();
            }
            return feedData;
        }
    }
}