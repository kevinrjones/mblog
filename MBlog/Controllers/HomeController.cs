using System.Collections.Generic;
using System.Web.Mvc;
using Logging;
using MBlog.Models.Home;
using MBlogModel;
using MBlogServiceInterfaces;

namespace MBlog.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;

        public HomeController(IPostService postService, IUserService userService, ILogger logger)
            : base(logger)
        {
            _postService = postService;
            _userService = userService;
        }

        public ActionResult Index()
        {
            //Logger.Info("HomeController Index method called");
            var model = new HomePageViewModel();

            GetUsersAndBlogs(model);
            GetPosts(model);

            return View(model);
        }

        private void GetPosts(HomePageViewModel homePageViewModel)
        {
            IEnumerable<Post> posts = _postService.GetBlogPosts();
            homePageViewModel.Add(posts);
        }

        private void GetUsersAndBlogs(HomePageViewModel homePageViewModel)
        {
            IEnumerable<User> users = _userService.GetUsersWithTheirBlogs();
            homePageViewModel.Add(users);
        }
    }
}