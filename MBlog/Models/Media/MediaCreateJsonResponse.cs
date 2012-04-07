using System;

namespace MBlog.Models.Media
{
    public class MediaCreateJsonResponse
    {
        public Exception exception;
        public string message;
        public bool success;
        public string url;
        public string action { get; set; }
        public string title { get; set; }
    }
}