using System.Collections.Generic;
using System.Web.Mvc;
using Logging;
using MBlog.Filters;
using MBlog.Models.Admin;
using MBlog.Models.Post;
using MBlog.Models.User;
using MBlogDomainInterfaces;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class DashboardController : BaseController
    {
        private IPostDomain _postDomain;
        private readonly IUserDomain _userDomain;

        public DashboardController(IPostDomain postDomain, IUserDomain userDomain, ILogger logger)
            : base(logger, null, null)
        {
            _postDomain = postDomain;
            _userDomain = userDomain;
        }

        [AuthorizeLoggedInUser]
        public ActionResult Index()
        {
            var userViewModel = HttpContext.User as UserViewModel;

            User user = _userDomain.GetUserWithTheirBlogs(userViewModel.Id);

            var adminUserViewModel = new AdminUserViewModel(userViewModel.Name, userViewModel.Id, user.Blogs);
            return View(adminUserViewModel);
        }

        [AuthorizeBlogOwner]
        public ActionResult ListPosts(AdminBlogViewModel model)
        {
            IList<Post> posts = _postDomain.GetOrderedBlogPosts(model.BlogId);
            var postsViewModel = new PostsViewModel (model.BlogId, model.Nickname, posts);
            return View(postsViewModel);
        }
    }
}