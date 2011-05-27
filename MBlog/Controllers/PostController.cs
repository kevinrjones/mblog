using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CodeKicker.BBCode;
using MBlog.Models;
using MBlog.Models.Comment;
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
        public PostController(IBlogRepository blogRepository, IPostRepository blogPostRepository, IUserRepository userRepository) : base(userRepository)
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
                return View(postsViewModel);
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
                CommentViewModel commentViewModel = new CommentViewModel {Name = name, Comment = BBCode.ToHtml(comment.CommentText), Commented = comment.Commented, EMail = comment.EMail};
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
