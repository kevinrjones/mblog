using System;
using System.IO;
using HtmlAgilityPack;
using MBlog.Models.Post;

namespace MBlog.Models.Home
{
    public class HomePagePostViewModel
    {
        public const int MaxLength = 300;
        private readonly PostViewModel _postViewModel;

        public HomePagePostViewModel(PostViewModel postViewModel)
        {
            _postViewModel = postViewModel;
        }

        public HomePagePostViewModel() : this(new PostViewModel()) { }

        public string Title
        {
            get { return _postViewModel.Title; }
            set { _postViewModel.Title = value; }
        }

        public string Post
        {
            get
            {
                //const int maxEntryLength = 200;
                if (_postViewModel.Post.Length > MaxLength)
                {
                    var doc = new HtmlDocument();

                    doc.OptionAutoCloseOnEnd = false;
                    doc.OptionFixNestedTags = true;
                    doc.OptionWriteEmptyNodes = true;
                    string post = _postViewModel.Post.Substring(0, MaxLength - 3) + "..."; 
                    doc.LoadHtml(post);
                    StringWriter writer = new StringWriter();
                    doc.Save(writer);                

                    return writer.ToString();
                }
                return _postViewModel.Post;
            }
            set { _postViewModel.Post = value; }
        }

        public DateTime DatePosted
        {
            get { return _postViewModel.DatePosted; }
            set { _postViewModel.DatePosted = value; }
        }

        public string UserName { get; set; }
    }
}