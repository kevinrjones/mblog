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
    public partial class CommentsController : BaseController
    {
        private readonly IPostService _postService;
        private readonly IBlogService _blogService;

        public CommentsController(IPostService postService, IBlogService blogService, ILogger logger) : base(logger)
        {
            _postService = postService;
            _blogService = blogService;
        }

        [AuthorizeBlogOwner]
        public virtual ActionResult Index(AdminBlogViewModel model)
        {
            var blog = _blogService.GetBlog(model.Nickname);
            IList<Post> posts = _postService.GetOrderedBlogPosts(blog.Id);
            var postsViewModel = new PostsViewModel(model.Nickname, posts);
            return View(postsViewModel);
        }
    }
}