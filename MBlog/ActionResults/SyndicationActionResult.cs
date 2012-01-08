using System;
using System.ServiceModel.Syndication;
using System.Web.Mvc;

namespace MBlog.ActionResults
{
    public class SyndicationActionResult : ActionResult
    {
        public FeedData FeedData { get; set; }

        public SyndicationActionResult(FeedData feedData)
        {
            FeedData = feedData;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            
            if (FeedData != null)
            {
                response.ContentType = FeedData.ContentType;
                response.AppendHeader("Cache-Control", "private");
                response.AppendHeader("Last-Modified", FeedData.LastModifiedDate.ToString("r"));
                response.AppendHeader("ETag", String.Format("\"{0}\"", FeedData.ETag));
                response.Output.WriteLine(FeedData.Content);
                response.StatusCode = 200;
                response.StatusDescription = "OK";
            }
        }
    }
}