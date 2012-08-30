using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Logging;
using MBlog.Models.Atom;
using MBlogServiceInterfaces;

namespace MBlog.Controllers.publishing
{
    public class AtomController : BaseController
    {
        private readonly IBlogService _blogService;
        private readonly IPostService _postService;

        public AtomController(IBlogService blogService, IPostService postService, ILogger logger) : base(logger)
        {
            _blogService = blogService;
            _postService = postService;
        }

        // GET /api/atom
        public ActionResult GetServiceDocument(string nickname)
        {
            var blog = _blogService.GetBlog(nickname);
            var viewModel = new AtomViewModel{Title = blog.Title, Nickname = nickname};
            return View(viewModel);
        }

    }
}
