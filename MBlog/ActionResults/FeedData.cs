using System;

namespace MBlog.ActionResults
{
    public class FeedData
    {
        public string Key { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string ETag { get; set; }
    }
}