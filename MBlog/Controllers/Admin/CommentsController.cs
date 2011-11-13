using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBlog.Filters;
using MBlog.Models.Admin;
using MBlog.Models.Post;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers.Admin
{
    public class CommentsController  : BaseController
    {
        private readonly IPostRepository _postRepository;

        public CommentsController(IUserRepository userRepository, IPostRepository postRepository,
                               IBlogRepository blogRepository)
            : base(userRepository, blogRepository)
        {
            _postRepository = postRepository;
        }

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
