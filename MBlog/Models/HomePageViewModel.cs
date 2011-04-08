using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBlog.Models
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