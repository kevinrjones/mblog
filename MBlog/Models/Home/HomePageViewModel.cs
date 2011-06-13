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
    }
}