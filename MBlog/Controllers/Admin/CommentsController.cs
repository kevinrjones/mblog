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
    public class CommentsController  : BaseController
    {
        private readonly IPostDomain _postDomain;

        public CommentsController(IPostDomain postDomain, ILogger logger) : base(logger)
        {
            _postDomain = postDomain;
        }

        [AuthorizeBlogOwner]
        public ActionResult Index(AdminBlogViewModel model)
        {
            var posts = _postDomain.GetOrderedBlogPosts(model.BlogId);
            var postsViewModel = new PostsViewModel (model.BlogId, model.Nickname, posts);
            return View(postsViewModel);
        }
    }
}
