using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logging;
using MBlog.Filters;
using MBlog.Models.Admin;
using MBlog.Models.Post;
using MBlogDomainInterfaces;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers.Admin
{
    public class PostsController : BaseController
    {
        private readonly IPostDomain _postDomain;

        public PostsController(IPostDomain postDomain, ILogger logger)
            : base(logger, null, null)
        {
            _postDomain = postDomain;
        }



        [AuthorizeBlogOwner]
        public ActionResult Index(AdminBlogViewModel model)
        {
            var posts = _postDomain.GetOrderedBlogPosts(model.BlogId);
            var postsViewModel = new PostsViewModel { BlogId = model.BlogId, Nickname = model.Nickname };
            postsViewModel.AddPosts(posts);
            return View(postsViewModel);
        }

    }
}
