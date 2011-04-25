using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBlog.Models
{
    public class PostLinkViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Nickname { get; set; }
        public string Link { get; set; }
    }
}