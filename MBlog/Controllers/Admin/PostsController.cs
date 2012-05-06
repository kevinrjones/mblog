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
    public partial class PostsController : BaseController
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService, ILogger logger)
            : base(logger)
        {
            _postService = postService;
        }


        [AuthorizeBlogOwner]
        public virtual ActionResult Index(AdminBlogViewModel model)
        {
            IList<Post> posts = _postService.GetOrderedBlogPosts(model.BlogId);
            var postsViewModel = new PostsViewModel {BlogId = model.BlogId, Nickname = model.Nickname};
            postsViewModel.AddPosts(posts);
            return View(postsViewModel);
        }
    }
}