using System.Collections.Generic;
using System.Web.Mvc;
using Logging;
using MBlog.Filters;
using MBlog.Models.Admin;
using MBlog.Models.Post;
using MBlog.Models.User;
using MBlogModel;
using MBlogServiceInterfaces;

namespace MBlog.Controllers
{
    public partial class DashboardController : BaseController
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;

        public DashboardController(IPostService postService, IUserService userService, ILogger logger)
            : base(logger)
        {
            _postService = postService;
            _userService = userService;
        }

        [AuthorizeLoggedInUser]
        public virtual ActionResult Index()
        {
            var userViewModel = HttpContext.User as UserViewModel;

            User user = _userService.GetUserWithTheirBlogs(userViewModel.Id);

            var adminUserViewModel = new AdminUserViewModel(userViewModel.Name, userViewModel.Id, user.Blogs);
            return View(adminUserViewModel);
        }

        [AuthorizeBlogOwner]
        public virtual ActionResult ListPosts(AdminBlogViewModel model)
        {
            IList<Post> posts = _postService.GetOrderedBlogPosts(model.BlogId);
            var postsViewModel = new PostsViewModel(model.BlogId, model.Nickname, posts);
            return View(postsViewModel);
        }
    }
}