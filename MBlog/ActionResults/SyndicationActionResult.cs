using System;
using System.ServiceModel.Syndication;
using System.Web.Mvc;

namespace MBlog.ActionResults
{
    public class SyndicationActionResult : ActionResult
    {
//        public SyndicationActionResult() { }
        public SyndicationActionResult(SyndicationFeed feed, Func<SyndicationFeed, FeedData> produceFeedData)
        {
            _produceFeedData = produceFeedData;
            Feed = feed;
        }

        public SyndicationFeed Feed { get; set; }
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
}