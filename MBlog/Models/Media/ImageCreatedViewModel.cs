using System;

namespace MBlog.Models.Media
{
    public class ImageCreatedViewModel 
    {
        public string ContentType { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public DateTime Edited { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string LinkKey { get; set; }
    }
}