using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBlog.Models;
using MBlogModel;
using MBlogRepository;

namespace MBlog.Controllers
{
    public class PostController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        //
        // GET: /Home/

        public PostController(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public ActionResult Index()
        {
            IList<Post> blogs = _blogPostRepository.GetBlogPosts();
            List<PostViewModel> viewModels = new List<PostViewModel>();

            if (blogs != null)
            {
                foreach (var blog in blogs)
                {
                    PostViewModel viewModel = new PostViewModel {Id = blog.Id, Post = blog.BlogPost, Title = blog.Title, DateLastEdited = blog.Edited, DatePosted = blog.Posted};
                    viewModels.Add(viewModel);
                }
            }
            return View(viewModels);
        }

    }
}
