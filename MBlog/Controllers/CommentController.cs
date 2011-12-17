using System.Web.Mvc;
using Logging;
using MBlog.Models.Comment;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class CommentController : BaseController
    {
        private readonly IPostRepository _postRepository;

        public CommentController(IPostRepository postRepository, IUserRepository userRepository,
                                 IBlogRepository blogRepository, ILogger logger)
            : base(logger, userRepository, blogRepository)
        {
            _postRepository = postRepository;
        }

        public ActionResult Create(AddCommentViewModel commentViewModel)
        {
            if (ModelState.IsValid)
            {
                _postRepository.AddComment(commentViewModel.PostId, commentViewModel.Name, commentViewModel.Comment);
            }
            else
            {
                TempData["comment"] = ViewData.ModelState;
            }
            return new RedirectResult(Request.Headers["Referer"]);
        }
    }
}