using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBlog.Models
{
    public class HomePagePostViewModel
    {
        public string Title { get; set; }
        public string Post { get; set; }
        public DateTime DatePosted { get; set; }
        public string UserName { get; set; }
    }
}