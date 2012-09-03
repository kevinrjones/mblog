using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Logging;
using MBlog.ActionResults;
using MBlog.Models.Atom;
using MBlog.Models.Feed;
using MBlog.Models.Post;
using MBlogModel;
using MBlogServiceInterfaces;

namespace MBlog.Controllers.publishing
{

    [BasicAuthorize]
    public class AtomController : BaseController
    {
        private readonly IBlogService _blogService;
        private readonly IPostService _postService;
        private readonly IDashboardService _dashboardService;
        private readonly ISyndicationFeedService _syndicationFeedService;

        public AtomController(IBlogService blogService, IPostService postService, IDashboardService dashboardService, ILogger logger, ISyndicationFeedService syndicationFeedService)
            : base(logger)
        {
            _blogService = blogService;
            _postService = postService;
            _dashboardService = dashboardService;
            _syndicationFeedService = syndicationFeedService;
        }

        // GET /api/atom
        [HttpGet]
        public ActionResult GetServiceDocument(string nickname)
        {
            var blog = _blogService.GetBlog(nickname);
            var viewModel = new AtomViewModel { Title = blog.Title, Nickname = nickname };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Index(string nickname)
        {
            SyndicationFeed feed = _syndicationFeedService.CreateSyndicationFeed(nickname, "atom",
                                                                     HttpContext.Request.Url.Scheme,
                                                                     HttpContext.Request.Headers["HOST"]);
            return new SyndicationActionResult(feed.GetAtomFeed());

        }

        [HttpGet]
        public ActionResult Get(string nickname, int blogId, int postId)
        {
            Post post = _postService.GetBlogPost(postId);
            return
                View(new EditPostViewModel { BlogId = blogId, PostId = postId, Title = post.Title, Post = post.BlogPost, Edited = post.Edited, Published = post.Posted});
        }
        
        [HttpPut]
        public ActionResult Update(string nickname, int blogId, int postId)
        {
            var atomXMl = XDocument.Load(new StreamReader(Request.InputStream));
            XNamespace ns = "http://www.w3.org/2005/Atom";
            var title = (from node in atomXMl.Descendants(ns + "title")
                           select node.Value).FirstOrDefault();
            var content = (from node in atomXMl.Descendants(ns + "content")
                           select node.Value).FirstOrDefault();
            _dashboardService.Update(postId, title, content, blogId);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        /*
         <?xml version="1.0" encoding="utf-8"?>
        <atom:entry xmlns:atom="http://www.w3.org/2005/Atom">
         <atom:id>urn:uuid:f289a4ae-901c-4209-9fa2-4ba987aec62a</atom:id>
         <atom:title>New</atom:title>
         <atom:content type="html">&lt;p&gt;New&lt;/p&gt;  </atom:content>
        </atom:entry>
         */
        [HttpPost]
        public ActionResult Create(string nickname)
        {
            var atomXMl = XDocument.Load(new StreamReader(Request.InputStream));
            XNamespace ns = "http://www.w3.org/2005/Atom";
            var title = (from node in atomXMl.Descendants(ns + "title")
                         select node.Value).FirstOrDefault();
            var content = (from node in atomXMl.Descendants(ns + "content")
                           select node.Value).FirstOrDefault();
            Blog blog = _blogService.GetBlog(nickname);
            Post post = new Post
            {
                BlogPost = content,
                Title = title,
                Edited = DateTime.UtcNow,
                Posted = DateTime.UtcNow,
                BlogId = blog.Id,
                CommentsEnabled = true,
            };
            _dashboardService.CreatePost(post, blog.Id);
            return View(new NewPostViewModel(post));
        }
    }
}
