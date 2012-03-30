using System.Web.Mvc;
using Logging;
using MBlog.Filters;
using MBlog.Models.Blog;
using MBlog.Models.User;
using MBlogModel;
using MBlogServiceInterfaces;

namespace MBlog.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService, ILogger logger)
            : base(logger)
        {
            _blogService = blogService;
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
            var user = HttpContext.User as UserViewModel;
            _blogService.CreateBlog(model.Title, model.Description, model.ApproveComments, model.CommentsEnabled,
                                   model.Nickname, user.Id);
            return RedirectToRoute(new { controller = "Dashboard", action = "Index" });
        }

        [HttpGet]
        [AuthorizeBlogOwner]
        public ActionResult Edit(CreateBlogViewModel model)
        {
            Blog blog = _blogService.GetBlog(model.Nickname);
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
            _blogService.UpdateBlog(model.Nickname, model.ApproveComments, model.CommentsEnabled, model.Description,
                                   model.Title);
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