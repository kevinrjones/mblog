using System.Web.Mvc;
using MBlog.Filters;
using MBlog.Models.Blog;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class BlogController : BaseController
    {
        public BlogController(IUserRepository userRepository, IBlogRepository blogRepository)
            : base(userRepository, blogRepository)
        {
        }

        [HttpGet]
        public ActionResult New()
        {
            if (RedirectIfInvalidUser())
                return RedirectToAction("New", "Session");

            return View(new CreateBlogViewModel {IsCreate = true});
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CreateBlogViewModel model)
        {
            if (RedirectIfInvalidUser())
                return RedirectToAction("New", "Session");

            if (!ModelState.IsValid)
            {
                return View("New", model);
            }
            return CreateBlog(model);
        }

        [HttpGet]
        [AuthorizeBlogOwner]
        public ActionResult Edit(CreateBlogViewModel model)
        {
            Blog blog = BlogRepository.GetBlog(model.Nickname);
            var modelOut = new CreateBlogViewModel(blog);
            return View(modelOut);
        }

        [HttpPost]
        [AuthorizeBlogOwner]
        public ActionResult Update(CreateBlogViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }
            return UpdateBlog(model);
        }

        private ActionResult UpdateBlog(CreateBlogViewModel model)
        {
            Blog blog = BlogRepository.GetBlog(model.Nickname);

            blog.ApproveComments = model.ApproveComments;
            blog.CommentsEnabled = model.CommentsEnabled;
            blog.Description = model.Description;
            blog.Title = model.Title;
            BlogRepository.Add(blog);
            return RedirectToRoute(new { controller = "Dashboard", action = "Index" });
        }

        private ActionResult CreateBlog(CreateBlogViewModel model)
        {
            var user = HttpContext.User as UserViewModel;
            var blog = new Blog(model.Title, model.Description, model.ApproveComments, model.CommentsEnabled,
                                model.Nickname, user.Id);
            BlogRepository.Create(blog);
            return RedirectToRoute(new { controller = "Dashboard", action = "Index" });
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