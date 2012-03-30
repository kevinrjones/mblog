using System.Collections.Generic;
using System.Web.Mvc;
using Logging;
using MBlog.Filters;
using MBlog.Models.Admin;
using MBlog.Models.Post;
using MBlogModel;
using MBlogServiceInterfaces;

namespace MBlog.Controllers.Admin
{
    public class CommentsController : BaseController
    {
        private readonly IPostService _postService;

        public CommentsController(IPostService postService, ILogger logger) : base(logger)
        {
            _postService = postService;
        }

        [AuthorizeBlogOwner]
        public ActionResult Index(AdminBlogViewModel model)
        {
            IList<Post> posts = _postService.GetOrderedBlogPosts(model.BlogId);
            var postsViewModel = new PostsViewModel(model.BlogId, model.Nickname, posts);
            return View(postsViewModel);
        }
    }
}