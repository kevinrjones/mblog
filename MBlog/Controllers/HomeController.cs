using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBlog.Models;
using MBlog.Models.Home;
using MBlog.Models.User;
using MBlogRepository;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPostRepository _postRepository;

        public HomeController(IUserRepository userRepository, IPostRepository postRepository, IBlogRepository blogRepository)
            : base(userRepository, blogRepository)
        {
            _postRepository = postRepository;
        }

        public ActionResult Index()
        {
            HomePageViewModel model = new HomePageViewModel();

            GetUsersAndBlogs(model);
            GetPosts(model);

            return View(model);
        }

        private void GetPosts(HomePageViewModel homePageViewModel)
        {
            var posts = _postRepository.GetPosts();
            homePageViewModel.Add(posts);
        }

        private void GetUsersAndBlogs(HomePageViewModel homePageViewModel)
        {
            var users = UserRepository.GetUsersWithTheirBlogs();
            homePageViewModel.Add(users);

        }
    }
}
