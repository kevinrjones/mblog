using System.Web.Mvc;
using Logging;
using MBlog.Models.Comment;
using MBlogDomainInterfaces;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class CommentController : BaseController
    {
        private IPostDomain _postDomain;

        public CommentController(IPostDomain postDomain, ILogger logger)
            : base(logger)
        {
            _postDomain = postDomain;
        }

        public ActionResult Create(AddCommentViewModel commentViewModel)
        {
            if (ModelState.IsValid)
            {
                _postDomain.AddComment(commentViewModel.PostId, commentViewModel.Name, commentViewModel.Comment);
            }
            else
            {
                TempData["comment"] = ViewData.ModelState;
            }
            return new RedirectResult(Request.Headers["Referer"]);
        }
    }
}