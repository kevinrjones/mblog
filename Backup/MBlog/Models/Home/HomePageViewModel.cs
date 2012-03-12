using System.Collections.Generic;
using MBlog.Models.User;

namespace MBlog.Models.Home
{
    public class HomePageViewModel
    {
        public HomePageViewModel()
        {
            UserBlogViewModels = new List<UserBlogViewModel>();
            HomePagePostViewModels = new List<HomePagePostViewModel>();
        }

        public List<UserBlogViewModel> UserBlogViewModels { get; set; }
        public List<HomePagePostViewModel> HomePagePostViewModels { get; set; }

        private void Add(MBlogModel.Post post)
        {
            var vm = new HomePagePostViewModel
                         {
                             Title = post.Title,
                             DatePosted = post.Posted,
                             Post = post.BlogPost,
                             UserName = post.Blog.User.Name
                         };

            HomePagePostViewModels.Add(vm);
        }

        public void Add(IEnumerable<MBlogModel.Post> posts)
        {
            foreach (MBlogModel.Post post in posts)
            {
                Add(post);
            }
        }

        public void Add(IEnumerable<MBlogModel.User> users)
        {
            foreach (MBlogModel.User user in users)
            {
                foreach (MBlogModel.Blog blog in user.Blogs)
                {
                    var viewModel = new UserBlogViewModel
                                        {Name = user.Name, Title = blog.Title, Nickname = blog.Nickname};
                    UserBlogViewModels.Add(viewModel);
                }
            }
        }
    }
}