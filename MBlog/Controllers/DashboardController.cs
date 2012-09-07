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
        private readonly IBlogService _blogService;

        public DashboardController(IPostService postService, IUserService userService, IBlogService blogService,  ILogger logger)
            : base(logger)
        {
            _postService = postService;
            _userService = userService;
            _blogService = blogService;
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
            var blog = _blogService.GetBlog(model.Nickname);
            IList<Post> posts = _postService.GetOrderedBlogPosts(blog.Id);
            var postsViewModel = new PostsViewModel(model.Nickname, posts);
            return View(postsViewModel);
        }
    }
}