﻿using System;
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
        private readonly IBlogPostRepository _blogPostRepository;
        //
        // GET: /Home/

        public HomeController(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public ActionResult Index()
        {
            IList<Post> blogs = _blogPostRepository.GetBlogPosts();
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
