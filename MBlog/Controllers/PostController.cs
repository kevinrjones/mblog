﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MBlog.Filters;
using MBlog.Models.Post;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    // todo: get all comments and hide them
    public class PostController : BaseController
    {
        private readonly IPostRepository _blogPostRepository;

        public PostController(IBlogRepository blogRepository, IPostRepository blogPostRepository,
                              IUserRepository userRepository)
            : base(userRepository, blogRepository)
        {
            BlogRepository = blogRepository;
            _blogPostRepository = blogPostRepository;
        }

        public ActionResult Index(string nickname)
        {
            var postsViewModel = new PostsViewModel();
            IList<Post> posts = _blogPostRepository.GetBlogPosts(nickname);
            postsViewModel.AddPosts(posts);
            return View(postsViewModel);
        }

        [HttpGet]
        [AuthorizeBlogOwner]
        public ActionResult New(string nickname, int blogId)
        {
            return View(new EditPostViewModel {BlogId = blogId, IsCreate = true});
        }

        [HttpPost]
        [ValidateInput(false)]
        [AuthorizeBlogOwner]
        public ActionResult Create(CreatePostViewModel model)
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
            Post post = _blogPostRepository.GetBlogPost(postId);
            return
                View(new EditPostViewModel {BlogId = blogId, PostId = postId, Title = post.Title, Post = post.BlogPost});
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

        public ActionResult Show(PostLinkViewModel postLinkViewModel)
        {
            var postsViewModel = new PostsViewModel();
            IList<Post> posts = _blogPostRepository.GetBlogPosts(postLinkViewModel.Year, postLinkViewModel.Month,
                                                                 postLinkViewModel.Day, postLinkViewModel.Nickname,
                                                                 postLinkViewModel.Link);

            var modelStateDictionary = TempData["comment"] as ModelStateDictionary;

            if (modelStateDictionary != null)
            {
                ViewData.ModelState.Merge(modelStateDictionary);
            }

            return GetPosts(postLinkViewModel, posts, postsViewModel);
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
                               CommentsEnabled = true,
                               //todo: get this from the admin
                           };
            _blogPostRepository.Create(post);
            return RedirectToRoute(new {controller = "Admin", action = "Index"});
        }

        private ActionResult UpdatePost(EditPostViewModel model)
        {
            Post post = _blogPostRepository.GetBlogPost(model.PostId);
            if (post == null)
            {
                throw new MBlogException("postId not valid");
            }
            post.UpdatePost(model.Title, model.Post);
            _blogPostRepository.Add(post);
            return RedirectToRoute(new {controller = "Admin", action = "Index"});
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