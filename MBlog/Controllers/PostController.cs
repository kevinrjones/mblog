using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Logging;
using MBlog.Filters;
using MBlog.Models.Post;
using MBlogModel;
using MBlogServiceInterfaces;

namespace MBlog.Controllers
{
    // todo: get all comments and hide them
    public class PostController : BaseController
    {
        private readonly IPostService _postService;
        private readonly IDashboardService _dashboardService;

        public PostController(IPostService postService, IDashboardService dashboardService, ILogger logger) : base(logger)
        {
            _postService = postService;
            _dashboardService = dashboardService;
        }

        public ActionResult Index(string nickname)
        {
            var postsViewModel = new PostsViewModel{Nickname = nickname};
            IList<Post> posts = _postService.GetBlogPosts(nickname);
            postsViewModel.AddPosts(posts);
            return View(postsViewModel);
        }

        [HttpGet]
        [AuthorizeBlogOwner]
        public ActionResult New(string nickname, int blogId)
        {
            return View(new EditPostViewModel {BlogId = blogId, IsCreate = true, Nickname = nickname});
        }

        [HttpPost]
        [ValidateInput(false)]
        [AuthorizeBlogOwner]
        public ActionResult Create(EditPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("New", model);
            }
            return CreatePost(model);
        }

        [HttpGet]
        [AuthorizeBlogOwner]
        public ActionResult Edit(string nickname, int blogId, int postId)
        {
            Post post = _postService.GetBlogPost(postId);
            return View(new EditPostViewModel {BlogId = blogId, PostId = postId, Title = post.Title, Post = post.BlogPost});
        }

        [HttpPost]
        [ValidateInput(false)]
        [AuthorizeBlogOwner]
        public ActionResult Update(EditPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }
            return UpdatePost(model);
        }

        [HttpPost]
        [AuthorizeBlogOwner]
        public ActionResult Delete(EditPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("InvalidDelete", model);
            }
            _postService.Delete(model.PostId);
            return RedirectToRoute(new {controller = "Posts", action = "Index"});
        }

        public ActionResult Show(PostLinkViewModel postLinkViewModel)
        {
            var postsViewModel = new PostsViewModel{Nickname = postLinkViewModel.Nickname};
            IList<Post> posts = _postService.GetBlogPosts(postLinkViewModel.Year, postLinkViewModel.Month,
                                                                 postLinkViewModel.Day, postLinkViewModel.Nickname,
                                                                 postLinkViewModel.Link);

            var modelStateDictionary = TempData["comment"] as ModelStateDictionary;

            if (modelStateDictionary != null)
            {
                ViewData.ModelState.Merge(modelStateDictionary);
            }

            return GetPosts(postLinkViewModel, posts, postsViewModel);
        }

        private ActionResult CreatePost(EditPostViewModel model)
        {
            var post = new Post
                           {
                               Title = model.Title,
                               BlogPost = model.Post,
                               Edited = DateTime.UtcNow,
                               Posted = DateTime.UtcNow,
                               BlogId = model.BlogId,
                               CommentsEnabled = true,
                               //todo: get this from the admin
                           };
            _dashboardService.CreatePost(post, model.BlogId);
            return RedirectToRoute(new { controller = "Dashboard", action = "Index" });
        }

        private ActionResult UpdatePost(EditPostViewModel model)
        {
            _dashboardService.Update(model.PostId, model.Title, model.Post, model.BlogId);
            return RedirectToRoute(new { controller = "Dashboard", action = "Index" });
        }

        private ActionResult GetPosts(PostLinkViewModel model, IList<Post> posts, PostsViewModel postsViewModel)
        {
            if (IsSinglePost(model, posts))
            {
                postsViewModel.ShowComments = true;
            }
            postsViewModel.AddPosts(posts);
            return View("Show", postsViewModel);
        }

        private bool IsSinglePost(PostLinkViewModel model, IEnumerable<Post> posts)
        {
            if (model.Year != 0
                && model.Month != 0
                && model.Day != 0
                && !string.IsNullOrEmpty(model.Link)
                && posts.Count() == 1)
            {
                return true;
            }
            return false;
        }
    }
}