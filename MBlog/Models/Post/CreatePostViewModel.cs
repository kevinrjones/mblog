using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using MBlog.Models.Validators;

namespace MBlog.Models.Post
{
    public class CreatePostViewModel
    {
        public string Nickname { get; set; }
        public int BlogId { get; set; }
        public string Title { get; set; }


        private string _post;
        public string Post
        {
            get { return _post; }
            set
            {
                var doc = new HtmlDocument();
                
                doc.OptionAutoCloseOnEnd = false;
                doc.OptionFixNestedTags = true;
                doc.OptionWriteEmptyNodes= true;
                doc.LoadHtml(value);
                StringWriter writer = new StringWriter();
                doc.Save(writer);                
                _post = writer.ToString();
            }
        }

        public bool IsCreate { get; set; }
    }
}