using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using HtmlAgilityPack;
using MBlog.Models.Media;

namespace MBlog.Models.Post
{
    public class EditPostViewModel : BasePostViewModel
    {
        private string _post;
        public int PostId { get; set; }
        public DateTime Edited { get; set; }
        public DateTime Published { get; set; }

        [Required]
        public string Title { get; set; }


        [Required]
        public string Post
        {
            get { return _post; }
            set
            {
                var doc = new HtmlDocument();

                doc.OptionAutoCloseOnEnd = false;
                doc.OptionFixNestedTags = true;
                doc.OptionWriteEmptyNodes = true;
                doc.LoadHtml(value);
                var writer = new StringWriter();
                doc.Save(writer);
                _post = writer.ToString();
            }
        }

        public bool IsCreate { get; set; }
        public NewMediaViewModel NewMediaViewModel { get; set; }
    }
}