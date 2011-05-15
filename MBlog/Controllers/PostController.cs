using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBlog.Models;
using MBlog.Models.Comment;
using MBlogModel;
using MBlogRepository;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostRepository _blogPostRepository;

        public PostController(IPostRepository blogPostRepository, IUserRepository userRepository) : base(userRepository)
        {
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
                    PostViewModel pvm = new PostViewModel {Id = post.Id, Post = post.BlogPost, Title = post.Title, DateLastEdited = post.Edited, DatePosted = post.Posted, Link = post.ToTitleLink()};
                    postsViewModel.Posts.Add(pvm);
                    AddComments(pvm, post);
                }
            }
            return View(postsViewModel);
        }

        public ActionResult Show(PostLinkViewModel model)
        {
            PostsViewModel postsViewModel = new PostsViewModel();
            IEnumerable<Post> posts = _blogPostRepository.GetBlogPosts(model.Year, model.Month, model.Day, model.Nickname, model.Link);


            // todo: should probably check this before setting up the view model
            // todo: return a view model appropriate to the view
            if (!ShouldShowComments(model, posts, postsViewModel))
            {
                foreach (var post in posts)
                {
                    PostViewModel pvm = new PostViewModel
                                            {
                                                Title = post.Title,
                                                Id = post.Id,
                                                DatePosted = post.Posted,
                                                DateLastEdited = post.Edited,
                                                Post = post.BlogPost,
                                                Link = post.ToTitleLink()
                                            };
                    postsViewModel.Posts.Add(pvm);
                    AddComments(pvm, post);
                }
                return View(postsViewModel);
            }
            else
            {
                // todo: Add ShowSinglePost to show comments
                var post = posts.FirstOrDefault();
                AddCommentViewModel acvm = new AddCommentViewModel
                                               {
                                                   Title = post.Title,
                                                   Id = post.Id,
                                                   DatePosted = post.Posted,
                                                   DateLastEdited = post.Edited,
                                                   Post = post.BlogPost,
                                                   Link = post.ToTitleLink()
                                               };
                AddComments(acvm, post);
                return View("ShowPostWithComments", acvm);
            }
        }

        private void AddComments(PostViewModel postViewModel, Post post)
        {
            foreach (Comment comment in post.Comments)
            {
                string name;
                name = string.IsNullOrEmpty(comment.Name) ? "Anonymous" : comment.Name;
                CommentViewModel commentViewModel = new CommentViewModel {Name = name, Comment = comment.CommentText, Commented = comment.Commented, EMail = comment.EMail};
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
