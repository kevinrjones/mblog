using System.Web.Mvc;
using Logging;
using MBlog.Models.Comment;
using MBlogServiceInterfaces;

namespace MBlog.Controllers
{
    public class CommentController : BaseController
    {
        private readonly IPostService _postService;

        public CommentController(IPostService postService, ILogger logger)
            : base(logger)
        {
            _postService = postService;
        }

        public ActionResult Create(AddCommentViewModel commentViewModel)
        {
            if (ModelState.IsValid)
            {
                _postService.AddComment(commentViewModel.PostId, commentViewModel.Name, commentViewModel.Comment);
            }
            else
            {
                TempData["comment"] = ViewData.ModelState;
            }
            return new RedirectResult(Request.Headers["Referer"]);
        }
    }
}