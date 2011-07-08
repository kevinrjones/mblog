using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CodeKicker.BBCode;
using MBlog.Models.Comment;
using MBlog.Models.Post;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    // todo: get all comments and hide them
    public class PostController : BaseController
    {
        private readonly IPostRepository _blogPostRepository;
        public PostController(IBlogRepository blogRepository, IPostRepository blogPostRepository, IUserRepository userRepository)
            : base(userRepository, blogRepository)
        {
            BlogRepository = blogRepository;
            _blogPostRepository = blogPostRepository;
        }

        public ActionResult Index(string nickname)
        {
            var postsViewModel = new PostsViewModel();
            var posts = _blogPostRepository.GetBlogPosts(nickname);

            if (posts != null)
            {
                foreach (var post in posts)
                {
                    var postViewModel = new PostViewModel(post);
                    postsViewModel.Posts.Add(postViewModel);
                }
            }
            return View(postsViewModel);
        }

        [HttpGet]
        public ActionResult New(string nickname, int blogId)
        {
            ActionResult redirectToAction;
            if (RedirectIfInvalidUser(nickname, blogId, out redirectToAction)) return redirectToAction;
            return View(new EditPostViewModel { BlogId = blogId, IsCreate = true });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CreatePostViewModel model)
        {
            ActionResult redirectToAction;
            if (RedirectIfInvalidUser(model.Nickname, model.BlogId, out redirectToAction)) return redirectToAction;

            if (!ModelState.IsValid)
            {
                return View("New", model);
            }
            return CreatePost(model);
        }

        [HttpGet]
        public ActionResult Edit(string nickname, int blogId, int postId)
        {
            ActionResult redirectToAction;
            if (RedirectIfInvalidUser(nickname, blogId, out redirectToAction)) return redirectToAction;
            Post post = _blogPostRepository.GetBlogPost(postId);
            return View(new EditPostViewModel { BlogId = blogId, PostId = postId, Title = post.Title, Post = post.BlogPost });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(EditPostViewModel model)
        {
            ActionResult redirectToAction;
            if (RedirectIfInvalidUser(model.Nickname, model.BlogId, out redirectToAction)) return redirectToAction;

            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }
            return UpdatePost(model);
        }

        public ActionResult Show(PostLinkViewModel postLinkViewModel)
        {
            var blog = BlogRepository.GetBlog(postLinkViewModel.Nickname);
            var postsViewModel = new PostsViewModel();
            var posts = _blogPostRepository.GetBlogPosts(postLinkViewModel.Year, postLinkViewModel.Month, postLinkViewModel.Day, postLinkViewModel.Nickname, postLinkViewModel.Link);

            var modelStateDictionary = TempData["comment"] as ModelStateDictionary;

            if (modelStateDictionary != null)
            {
                ViewData.ModelState.Merge(modelStateDictionary);
            }

            return GetPosts(postLinkViewModel, blog, posts, postsViewModel);
        }

        private ActionResult CreatePost(CreatePostViewModel model)
        {
            var post = new Post
            {
                Title = model.Title,
                BlogPost = model.Post,
                Edited = DateTime.UtcNow,
                Posted = DateTime.UtcNow,
                BlogId = model.BlogId,
                CommentsEnabled = true, //todo: get this from the admin
            };
            _blogPostRepository.Create(post);
            return RedirectToRoute(new { controller = "admin", action = "Index" });
        }

        private ActionResult UpdatePost(EditPostViewModel model)
        {
            var post = _blogPostRepository.GetBlogPost(model.PostId);
            if (post == null)
            {
                throw new MBlogException("postId not valid");
            }
            post.UpdatePost(model.Title, model.Post);
            _blogPostRepository.Add(post);
            return RedirectToRoute(new { controller = "admin", action = "Index" });
        }

        private ActionResult GetPosts(PostLinkViewModel model, Blog blog, IList<Post> posts, PostsViewModel postsViewModel)
        {
            if (IfSinglePost(model, posts, postsViewModel))
            {
                var post = posts.FirstOrDefault();
                var postViewModel = new PostViewModel(post);
                postViewModel.CommentsEnabled = post.CommentsEnabled && blog.CommentsEnabled;
                postsViewModel.Posts.Add(postViewModel);
                return View("Show", postsViewModel);
            }

            foreach (var post in posts)
            {
                var postViewModel = new PostViewModel(post);
                postsViewModel.Posts.Add(postViewModel);
            }
            return View("Show", postsViewModel);
        }

        private bool IfSinglePost(PostLinkViewModel model, IEnumerable<Post> posts, PostsViewModel postsViewModel)
        {
            if (model.Year != 0
                && model.Month != 0
                && model.Day != 0
                && !string.IsNullOrEmpty(model.Link)
                && posts.Count() == 1)
            {
                postsViewModel.ShowComments = true;
            }
            return postsViewModel.ShowComments;
        }
    }
}
