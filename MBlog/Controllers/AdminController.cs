using System.Collections.Generic;
using System.Web.Mvc;
using MBlog.Models.Admin;
using MBlog.Models.Post;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IPostRepository _postRepository;

        public AdminController(IUserRepository userRepository, IPostRepository postRepository,
                               IBlogRepository blogRepository)
            : base(userRepository, blogRepository)
        {
            _postRepository = postRepository;
        }

        public ActionResult Index()
        {
            var user = HttpContext.User as UserViewModel;
            if (!IsLoggedInUser(user)) return RedirectToAction("login", "User");

            User users = UserRepository.GetUserWithTheirBlogs(user.Id);
            var adminUserViewModel = new AdminUserViewModel {Name = user.Name, UserId = user.Id};
            adminUserViewModel.AddBlogs(users.Blogs);
            return View(adminUserViewModel);
        }

        public ActionResult ListPosts(AdminBlogViewModel model)
        {
            ActionResult redirectToAction;
            if (RedirectIfInvalidUser(model.Nickname, model.BlogId, out redirectToAction)) return redirectToAction;
            IList<Post> posts = _postRepository.GetOrderedBlogPosts(model.BlogId);
            var postsViewModel = new PostsViewModel {BlogId = model.BlogId, Nickname = model.Nickname};
            postsViewModel.AddPosts(posts);
            return View(postsViewModel);
        }

        //public ActionResult ListComments(string nickname, int blogId)
        //{
        //    ActionResult redirectToAction;
        //    if (RedirectIfInvalidUser(nickname, blogId, out redirectToAction)) return redirectToAction;
        //    var posts = _postRepository.GetBlogPosts(blogId);
        //    // todo: list comments not posts
        //    PostsViewModel postsViewModel = new PostsViewModel { BlogId = blogId, Nickname = nickname};
        //    foreach (var post in posts)
        //    {
        //        PostViewModel pvm = new PostViewModel(post);
        //        postsViewModel.Posts.Add(pvm);
        //    }
        //    return View(postsViewModel);
        //}
    }
}