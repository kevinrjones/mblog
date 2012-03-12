using System.Collections.Generic;
using System.Web.Mvc;
using Logging;
using MBlog.Models.Home;
using MBlogDomainInterfaces;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class HomeController : BaseController
    {
        private IPostDomain _postDomain;
        private IUserDomain _userDomain;

        public HomeController(IPostDomain postDomain, IUserDomain userDomain, ILogger logger)
            : base(logger)
        {
            _postDomain = postDomain;
            _userDomain = userDomain;
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
            IEnumerable<Post> posts = _postDomain.GetBlogPosts();
            homePageViewModel.Add(posts);
        }

        private void GetUsersAndBlogs(HomePageViewModel homePageViewModel)
        {
            IEnumerable<User> users = _userDomain.GetUsersWithTheirBlogs();
            homePageViewModel.Add(users);
        }
    }
}