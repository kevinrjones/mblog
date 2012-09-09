using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Logging;
using MBlog.Filters;
using MBlog.Models.Media;
using MBlog.Models.Post;
using MBlogModel;
using MBlogServiceInterfaces;

namespace MBlog.Controllers
{
    // todo: get all comments and hide them
    public partial class PostController : BaseController
    {
        private readonly IDashboardService _dashboardService;
        private readonly IPostService _postService;
        private readonly IBlogService _blogService;

        public PostController(IPostService postService, IDashboardService dashboardService, IBlogService blogService, ILogger logger)
            : base(logger)
        {
            _postService = postService;
            _dashboardService = dashboardService;
            _blogService = blogService;
        }

        public virtual ActionResult Index(string nickname)
        {
            var postsViewModel = new PostsViewModel { Nickname = nickname };
            IList<Post> posts = _postService.GetBlogPosts(nickname);
            postsViewModel.AddPosts(posts);
            return View(postsViewModel);
        }

        [HttpGet]
        [AuthorizeBlogOwner]
        public virtual ActionResult New(string nickname)
        {
            NewMediaViewModel model = new NewMediaViewModel { Nickname = nickname };
            return View(new EditPostViewModel { IsCreate = true, Nickname = nickname, NewMediaViewModel = model });
        }

        [HttpPost]
        [ValidateInput(false)]
        [AuthorizeBlogOwner]
        public virtual ActionResult Create(string nickname, EditPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("New", model);
            }
            return CreatePost(nickname, model);
        }

        [HttpGet]
        [AuthorizeBlogOwner]
        public virtual ActionResult Edit(string nickname, int postId)
        {
            Post post = _postService.GetBlogPost(postId);
            return
                View(new EditPostViewModel { PostId = postId, Title = post.Title, Post = post.BlogPost });
        }

        [HttpPost]
        [ValidateInput(false)]
        [AuthorizeBlogOwner]
        public virtual ActionResult Update(string nickname, EditPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }
            return UpdatePost(nickname, model);
        }

        [HttpPost]
        [AuthorizeBlogOwner]
        public virtual ActionResult Delete(string nickname, int postId)
        {
            try
            {
                _postService.Delete(postId);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        public virtual ActionResult Show(PostLinkViewModel postLinkViewModel)
        {
            var postsViewModel = new PostsViewModel { Nickname = postLinkViewModel.Nickname };
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

        private ActionResult CreatePost(string nickname, EditPostViewModel model)
        {
            var blog = _blogService.GetBlog(nickname);
            var post = new Post
                           {
                               Title = model.Title,
                               BlogPost = model.Post,
                               Edited = DateTime.UtcNow,
                               Posted = DateTime.UtcNow,
                               BlogId = blog.Id,
                               CommentsEnabled = true,
                               //todo: get this from the admin
                           };
            _dashboardService.CreatePost(post, blog.Id);
            return RedirectToRoute(new { controller = "Dashboard", action = "Index" });
        }

        private ActionResult UpdatePost(string nickname, EditPostViewModel model)
        {
            var blog = _blogService.GetBlog(nickname);
            _dashboardService.Update(model.PostId, model.Title, model.Post, blog.Id);
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