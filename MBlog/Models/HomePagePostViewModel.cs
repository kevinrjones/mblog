using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBlog.Models
{
    public class HomePagePostViewModel
    {
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
                // todo: trim this to 200 but remember to leave in the closing html tags
                return _postViewModel.Post;
            }
            set { _postViewModel.Post = value; }
        }

        public DateTime DatePosted
        {
            get { return _postViewModel.DatePosted; }
            set { _postViewModel.DatePosted = value; }
        }

        public string Link
        {
            get { return _postViewModel.Link; }
        }

        public string UserName { get; set; }
    }
}