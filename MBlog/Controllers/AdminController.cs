using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBlog.Models;
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

        public AdminController(IUserRepository userRepository, IPostRepository postRepository, IBlogRepository blogRepository)
            : base(userRepository, blogRepository)
        {
            _postRepository = postRepository;
        }

        public ActionResult Index()
        {
            UserViewModel user = HttpContext.User as UserViewModel;
            if (!IsLoggedInUser(user))
            {
                return RedirectToAction("login", "user");
            }
            var users = UserRepository.GetUserWithTheirBlogs(user.Id);
            AdminUserViewModel adminUserViewModel = new AdminUserViewModel { Name = user.Name, UserId = user.Id};
            foreach (Blog blog in users.Blogs)
            {
                adminUserViewModel.Blogs.Add(new AdminBlogViewModel
                                                 {
                                                     BlogId = blog.Id,
                                                     Nickname = blog.Nickname,
                                                     Title = blog.Title,
                                                     Description = blog.Description
                                                 });
            }
            return View(adminUserViewModel);
        }

        public ActionResult ListPosts(int blogId)
        {
            UserViewModel user = HttpContext.User as UserViewModel;
            if (!IsLoggedInUser(user))
            {
                return RedirectToAction("login", "user");
            }
            var posts = _postRepository.GetBlogPosts(blogId);
            PostsViewModel postsViewModel = new PostsViewModel{BlogId = blogId};
            foreach (var post in posts)
            {
                PostViewModel pvm = new PostViewModel(post);
                postsViewModel.Posts.Add(pvm);
            }
            return View(postsViewModel);
        }

        public ActionResult ListComments(int blogId)
        {
            UserViewModel user = HttpContext.User as UserViewModel;
            if (!IsLoggedInUser(user))
            {
                return RedirectToAction("login", "user");
            }
            var posts = _postRepository.GetBlogPosts(blogId);
            PostsViewModel postsViewModel = new PostsViewModel { BlogId = blogId };
            foreach (var post in posts)
            {
                PostViewModel pvm = new PostViewModel(post);
                postsViewModel.Posts.Add(pvm);
            }
            return View(postsViewModel);
        }
    }
}
