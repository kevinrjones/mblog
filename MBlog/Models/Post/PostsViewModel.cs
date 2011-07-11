using System.Collections.Generic;

namespace MBlog.Models.Post
{
    public class PostsViewModel
    {
        private List<PostViewModel> _posts = new List<PostViewModel>();
        
        public List<PostViewModel> Posts
        {
            get { return _posts; }
            set { _posts = value; }
        }

        public bool ShowComments { get; set; }

        public int BlogId { get; set; }
        public string Nickname { get; set; }

        public void AddPosts(IList<MBlogModel.Post> posts)
        {
            foreach (var post in posts)
            {
                PostViewModel pvm = new PostViewModel(post);
                pvm.CommentsEnabled = post.CommentsEnabled && post.Blog.CommentsEnabled;
                Posts.Add(pvm);
            }
        }
    }
}