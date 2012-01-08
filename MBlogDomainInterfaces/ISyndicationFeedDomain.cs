using System.ServiceModel.Syndication;

namespace MBlogDomainInterfaces
{
    public interface ISyndicationFeedDomain
    {
        SyndicationFeed CreateSyndicationFeed(string nickname, string feedType, string scheme, string host);
    }
}