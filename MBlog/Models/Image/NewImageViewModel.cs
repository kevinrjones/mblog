using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBlog.Models.Image
{
    public class NewImageViewModel
    {
        public int BlogId { get; set; }
        public string Nickname { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Alternate { get; set; }
        public string Alignment { get; set; }
        public string Size { get; set; }
    }
}