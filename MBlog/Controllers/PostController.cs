using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBlog.Models;
using MBlogModel;
using MBlogRepository;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class PostController : BaseController
    {
        private readonly IBlogPostRepository _blogPostRepository;

        public PostController(IBlogPostRepository blogPostRepository, IUserRepository userRepository) : base(userRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public ActionResult Index(string nickname)
        {
            IList<Post> posts = _blogPostRepository.GetBlogPosts(nickname);
            List<PostViewModel> viewModels = new List<PostViewModel>();

            if (posts != null)
            {
                foreach (var post in posts)
                {
                    PostViewModel viewModel = new PostViewModel {Id = post.Id, Post = post.BlogPost, Title = post.Title, DateLastEdited = post.Edited, DatePosted = post.Posted};
                    viewModels.Add(viewModel);
                }
            }
            return View(viewModels);
        }

        public ActionResult Show(PostLinkViewModel model)
        {
            List<PostViewModel> viewModel = new List<PostViewModel>();            
            IEnumerable<Post> posts = _blogPostRepository.GetBlogPosts(model.Year, model.Month, model.Day, model.Title);

            foreach (var post in posts)
            {
                viewModel.Add(new PostViewModel{Title = post.Title, Id = post.Id, DatePosted = post.Posted, DateLastEdited = post.Edited});
            }
            return View(viewModel);
        }
    }
}
