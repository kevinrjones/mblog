using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBlog.Models.Blog;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class BlogController : BaseController
    {
        public BlogController(IUserRepository userRepository, IBlogRepository blogRepository) : base(userRepository, blogRepository){}

        [HttpGet]
        public ActionResult New()
        {
            if (RedirectIfInvalidUser()) 
                return RedirectToAction("login", "user");
            
            return View(new CreateBlogViewModel { IsCreate = true });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CreateBlogViewModel model)
        {
            if (RedirectIfInvalidUser()) 
                return RedirectToAction("login", "user");

            if (!ModelState.IsValid)
            {
                return View("New", model);
            }
            return CreateBlog(model);
        }

        private ActionResult CreateBlog(CreateBlogViewModel model)
        {
            var user = HttpContext.User as UserViewModel;
            var blog = new Blog
                           {
                               Title = model.Title,
                               Description = model.Description,
                               ApproveComments = model.ApproveComments,
                               CommentsEnabled = model.CommentsEnabled,
                               Nickname = model.Nickname,
                               UserId = user.Id
                           };
            BlogRepository.Create(blog);
            return RedirectToRoute(new { controller = "admin", action = "Index" });

        }

        private bool RedirectIfInvalidUser()
        {
            var user = HttpContext.User as UserViewModel;
            if (!IsLoggedInUser(user))
            {
                return true;
            }
            return false;
        }

    }
}
