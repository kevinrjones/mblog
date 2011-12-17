using System.Collections.Generic;
using System.Web.Mvc;
using Logging;
using MBlog.Filters;
using MBlog.Models.Admin;
using MBlog.Models.Post;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IPostRepository _postRepository;

        public DashboardController(IUserRepository userRepository, IPostRepository postRepository,
                               IBlogRepository blogRepository, ILogger logger)
            : base(logger, userRepository, blogRepository)
        {
            _postRepository = postRepository;
        }

        [AuthorizeLoggedInUser]
        public ActionResult Index()
        {
            var user = HttpContext.User as UserViewModel;

            User users = UserRepository.GetUserWithTheirBlogs(user.Id);
            var adminUserViewModel = new AdminUserViewModel {Name = user.Name, UserId = user.Id};
            adminUserViewModel.AddBlogs(users.Blogs);
            return View(adminUserViewModel);
        }

        [AuthorizeBlogOwner]
        public ActionResult ListPosts(AdminBlogViewModel model)
        {
            var posts = _postRepository.GetOrderedBlogPosts(model.BlogId);
            var postsViewModel = new PostsViewModel {BlogId = model.BlogId, Nickname = model.Nickname};
            postsViewModel.AddPosts(posts);
            return View(postsViewModel);
        }
    }
}