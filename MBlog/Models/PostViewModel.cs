using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBlog.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Post { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime? DateLastEdited { get; set; }
    }
}