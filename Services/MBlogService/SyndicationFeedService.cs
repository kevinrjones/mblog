using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using MBlogModel;
using MBlogRepository.Interfaces;
using MBlogServiceInterfaces;

namespace MBlogService
{
    public class SyndicationFeedService : ISyndicationFeedService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IPostRepository _postRepository;

        public SyndicationFeedService(IBlogRepository blogRepository, IPostRepository postRepository)
        {
            _blogRepository = blogRepository;
            _postRepository = postRepository;
        }

        #region ISyndicationFeedService Members

        public SyndicationFeed CreateSyndicationFeed(string nickname, string feedType, string scheme, string host)
        {
            IList<Post> posts = _postRepository.GetBlogPosts(nickname);
            Blog blog = _blogRepository.GetBlog(nickname);

            string url = string.Format("{0}://{1}/{2}", scheme, host, nickname);
            var feed = new SyndicationFeed(blog.Title, blog.Description, new Uri(url), url, blog.LastUpdated);
            feed.Authors.Add(new SyndicationPerson {Name = blog.User.Name});
            feed.Links.Add(SyndicationLink.CreateSelfLink(new Uri(url + "/feed/" + feedType)));

            var items = new List<SyndicationItem>();
            foreach (Post post in posts)
            {
                url = string.Format("{0}://{1}/{2}/{3}/{4}/{5}/{6}", scheme, host, nickname, post.Posted.Year,
                                    post.Posted.Month, post.Posted.Day, post.TitleLink);

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

        #endregion
    }
}