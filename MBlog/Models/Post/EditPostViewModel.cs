using System.IO;
using HtmlAgilityPack;

namespace MBlog.Models.Post
{
    public class EditPostViewModel : BasePostViewModel
    {
        private string _post;
        public int BlogId { get; set; }
        public int PostId { get; set; }

        public string Title { get; set; }


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

    }
}