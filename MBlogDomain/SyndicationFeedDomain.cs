using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using MBlogDomainInterfaces;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlogDomain
{
    public class SyndicationFeedDomain : ISyndicationFeedDomain
    {
        private readonly IPostRepository _postRepository;
        private readonly IBlogRepository _blogRepository;

        public SyndicationFeedDomain(IBlogRepository blogRepository, IPostRepository postRepository)
        {
            _blogRepository = blogRepository;
            _postRepository = postRepository;
        }

        public SyndicationFeed CreateSyndicationFeed(string nickname, string feedType, string scheme, string host)
        {
            IList<Post> posts = _postRepository.GetBlogPosts(nickname);
            var blog = _blogRepository.GetBlog(nickname);

            string url = string.Format("{0}://{1}/{2}", scheme, host, nickname);
            var feed = new SyndicationFeed(blog.Title, blog.Description, new Uri(url), url, blog.LastUpdated);
            feed.Authors.Add(new SyndicationPerson { Name = blog.User.Name });
            feed.Links.Add(SyndicationLink.CreateSelfLink(new Uri(url + "/feed/" + feedType)));

            var items = new List<SyndicationItem>();
            foreach (var post in posts)
            {
                url = string.Format("{0}://{1}/{2}/{3}/{4}/{5}/{6}", scheme, host, nickname, post.Posted.Year, post.Posted.Month, post.Posted.Day, post.TitleLink);

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
