using System.Collections.Generic;

namespace MBlog.Models.Post
{
    public class PostsViewModel : BasePostViewModel
    {
        private List<PostViewModel> _posts = new List<PostViewModel>();

        public PostsViewModel()
        {
        }

        public PostsViewModel(string nickname, IList<MBlogModel.Post> posts)
        {
            Nickname = nickname;
            AddPosts(posts);
        }

        public List<PostViewModel> Posts
        {
            get { return _posts; }
            set { _posts = value; }
        }

        public bool ShowComments { get; set; }

        public void AddPosts(IList<MBlogModel.Post> posts)
        {
            foreach (MBlogModel.Post post in posts)
            {
                var pvm = new PostViewModel(post);
                pvm.CommentsEnabled = post.CommentsEnabled && post.Blog.CommentsEnabled;
                Posts.Add(pvm);
            }
        }
    }
}