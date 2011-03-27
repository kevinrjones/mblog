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
    public class HomeController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        //
        // GET: /Home/

        public HomeController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public ActionResult Index()
        {
            IList<Post> blogs = _blogRepository.GetBlogs();
            List<BlogViewModel> viewModels = new List<BlogViewModel>();

            if (blogs != null)
            {
                foreach (var blog in blogs)
                {
                    BlogViewModel viewModel = new BlogViewModel {Id = blog.Id, Post = blog.BlogPost, Title = blog.Title, DateLastEdited = blog.Edited, DatePosted = blog.Posted};
                    viewModels.Add(viewModel);
                }
            }
            return View(viewModels);
        }

    }
}
