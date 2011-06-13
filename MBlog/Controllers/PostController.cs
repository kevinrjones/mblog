using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CodeKicker.BBCode;
using MBlog.Models;
using MBlog.Models.Comment;
using MBlog.Models.Post;
using MBlogModel;
using MBlogRepository.Interfaces;
using MBlogRepository.Repositories;

namespace MBlog.Controllers
{
    // todo: get all comments and hide them
    public class PostController : BaseController
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IPostRepository _blogPostRepository;
        private Blog blog;
        public PostController(IBlogRepository blogRepository, IPostRepository blogPostRepository, IUserRepository userRepository)
            : base(userRepository)
        {
            _blogRepository = blogRepository;
            _blogPostRepository = blogPostRepository;
        }

        public ActionResult Index(string nickname)
        {
            PostsViewModel postsViewModel = new PostsViewModel();
            IList<Post> posts = _blogPostRepository.GetBlogPosts(nickname);

            if (posts != null)
            {
                foreach (var post in posts)
                {
                    PostViewModel pvm = CreatePostViewModel(post);
                    postsViewModel.Posts.Add(pvm);
                    AddComments(pvm, post);
                }
            }
            return View(postsViewModel);
        }

        [HttpGet]
        public ActionResult New(string nickname, int blogId)
        {
            ActionResult redirectToAction;
            if (RedirectIfInvalidUser(nickname, blogId, out redirectToAction)) return redirectToAction;
            return View(new CreatePostViewModel { BlogId = blogId });
        }

        [HttpPost]
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

        public ActionResult Show(PostLinkViewModel postLinkViewModel)
        {
            blog = _blogRepository.GetBlog(postLinkViewModel.Nickname);
            PostsViewModel postsViewModel = new PostsViewModel();
            IEnumerable<Post> posts = _blogPostRepository.GetBlogPosts(postLinkViewModel.Year, postLinkViewModel.Month, postLinkViewModel.Day, postLinkViewModel.Nickname, postLinkViewModel.Link);
            //string key = PassStateAttribute.TempDataTransferKey; 
            ModelStateDictionary modelStateDictionary = TempData["comment"] as ModelStateDictionary;

            if (modelStateDictionary != null)
            {
                ViewData.ModelState.Merge(modelStateDictionary);
            }

            return GetPosts(postLinkViewModel, blog, posts, postsViewModel);
        }

        private bool RedirectIfInvalidUser(string nickname, int blogId, out ActionResult redirectToAction)
        {
            UserViewModel user = HttpContext.User as UserViewModel;
            if (!IsLoggedInUser(user) || !UserOwnsBlog(nickname, blogId))
            {
                redirectToAction = RedirectToAction("login", "user");
                return true;
            }
            redirectToAction = null;
            return false;
        }

        private ActionResult CreatePost(CreatePostViewModel model)
        {
            Post post = new Post
            {
                Title = model.Title,
                BlogPost = model.Post,
                Edited = DateTime.UtcNow,
                Posted = DateTime.UtcNow,
                BlogId = model.BlogId
            };
            _blogPostRepository.Create(post);
            return RedirectToRoute(new { controller = "admin", action="Index" });
        }

        private bool UserOwnsBlog(string nickname, int blogId)
        {
            var blog = _blogRepository.GetBlog(nickname);
            return blog.Id == blogId;
        }

        private PostViewModel CreatePostViewModel(Post post)
        {
            return new PostViewModel { Id = post.Id, Post = post.BlogPost, Title = post.Title, DateLastEdited = post.Edited, DatePosted = post.Posted, Link = post.TitleLink };
        }

        private ActionResult GetPosts(PostLinkViewModel model, Blog blog, IEnumerable<Post> posts, PostsViewModel postsViewModel)
        {
            if (!ShouldShowComments(model, posts, postsViewModel))
            {
                foreach (var post in posts)
                {
                    PostViewModel pvm = CreatePostViewModel(post);
                    AddComments(pvm, post);
                    postsViewModel.Posts.Add(pvm);
                }
                return View("Show", postsViewModel);
            }
            else
            {
                var post = posts.FirstOrDefault();
                AddCommentViewModel acvm = CreateAddCommentViewModel(post);
                blog = _blogRepository.GetBlog(model.Nickname);
                acvm.CommentsEnabled = blog.CommentsEnabled;
                AddComments(acvm, post);
                return View("ShowPostWithComments", acvm);
            }
        }

        private AddCommentViewModel CreateAddCommentViewModel(Post post)
        {
            return new AddCommentViewModel
                       {
                           Title = post.Title,
                           Id = post.Id,
                           DatePosted = post.Posted,
                           DateLastEdited = post.Edited,
                           Post = post.BlogPost,
                           Link = post.TitleLink
                       };
        }

        private void AddComments(PostViewModel postViewModel, Post post)
        {
            foreach (Comment comment in post.ApprovedComments)
            {
                string name;
                name = string.IsNullOrEmpty(comment.Name) ? "Anonymous" : comment.Name;
                CommentViewModel commentViewModel = new CommentViewModel { Name = name, Comment = BBCode.ToHtml(comment.CommentText), Commented = comment.Commented, EMail = comment.EMail };
                postViewModel.Comments.Add(commentViewModel);
            }
        }

        private bool ShouldShowComments(PostLinkViewModel model, IEnumerable<Post> posts, PostsViewModel postsViewModel)
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
