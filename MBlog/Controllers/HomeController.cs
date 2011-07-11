using System.Collections.Generic;
using System.Web.Mvc;
using MBlog.Models.Home;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPostRepository _postRepository;

        public HomeController(IUserRepository userRepository, IPostRepository postRepository,
                              IBlogRepository blogRepository)
            : base(userRepository, blogRepository)
        {
            _postRepository = postRepository;
        }

        public ActionResult Index()
        {
            var model = new HomePageViewModel();

            GetUsersAndBlogs(model);
            GetPosts(model);

            return View(model);
        }

        private void GetPosts(HomePageViewModel homePageViewModel)
        {
            IEnumerable<Post> posts = _postRepository.GetPosts();
            homePageViewModel.Add(posts);
        }

        private void GetUsersAndBlogs(HomePageViewModel homePageViewModel)
        {
            IEnumerable<User> users = UserRepository.GetUsersWithTheirBlogs();
            homePageViewModel.Add(users);
        }
    }
}