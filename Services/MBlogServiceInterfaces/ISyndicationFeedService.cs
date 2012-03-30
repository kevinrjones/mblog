using System.ServiceModel.Syndication;

namespace MBlogServiceInterfaces
{
    public interface ISyndicationFeedService
    {
        SyndicationFeed CreateSyndicationFeed(string nickname, string feedType, string scheme, string host);
    }
}