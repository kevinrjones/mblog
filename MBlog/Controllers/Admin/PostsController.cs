using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logging;
using MBlog.Filters;
using MBlog.Models.Admin;
using MBlog.Models.Post;
using MBlogRepository.Interfaces;
using MBlogServiceInterfaces;

namespace MBlog.Controllers.Admin
{
    public class PostsController : BaseController
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService, ILogger logger)
            : base(logger)
        {
            _postService = postService;
        }



        [AuthorizeBlogOwner]
        public ActionResult Index(AdminBlogViewModel model)
        {
            var posts = _postService.GetOrderedBlogPosts(model.BlogId);
            var postsViewModel = new PostsViewModel { BlogId = model.BlogId, Nickname = model.Nickname };
            postsViewModel.AddPosts(posts);
            return View(postsViewModel);
        }

    }
}
