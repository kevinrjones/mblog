using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBlog.Models.Comment;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class CommentController : BaseController
    {
        private readonly IPostRepository _blogPostRepository;

        public CommentController(IPostRepository blogPostRepository, IUserRepository userRepository)
            : base(userRepository)
        {
            _blogPostRepository = blogPostRepository;
        }
        public ActionResult Index()
        {
            return new ContentResult();
        }

        public ActionResult New(AddCommentViewModel commentViewModel)
        {
            if (ModelState.IsValid)
            {
                _blogPostRepository.AddComment(commentViewModel.Id, commentViewModel.Name, commentViewModel.Comment);
            }
            return new RedirectResult(Request.Headers["Referer"]);
        }
    }
}
