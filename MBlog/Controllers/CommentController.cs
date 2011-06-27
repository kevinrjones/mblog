using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeKicker.BBCode;
using MBlog.Models.Comment;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class CommentController : BaseController
    {
        private readonly IPostRepository _postRepository;

        public CommentController(IPostRepository postRepository, IUserRepository userRepository)
            : base(userRepository)
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
