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

namespace MBlog.Controllers.Admin
{
    public class PostsController  : BaseController
    {
        private readonly IPostRepository _postRepository;

        public PostsController(IUserRepository userRepository, IPostRepository postRepository,
                               IBlogRepository blogRepository, ILogger logger)
            : base(logger, userRepository, blogRepository)
        {
            _postRepository = postRepository;
        }
        //
        // GET: /Posts/

        [AuthorizeBlogOwner]
        public ActionResult Index(AdminBlogViewModel model)
        {
            var posts = _postRepository.GetOrderedBlogPosts(model.BlogId);
            var postsViewModel = new PostsViewModel { BlogId = model.BlogId, Nickname = model.Nickname };
            postsViewModel.AddPosts(posts);
            return View(postsViewModel);
        }

    }
}
