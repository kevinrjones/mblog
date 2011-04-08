using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBlog.Models;
using MBlogRepository;

namespace MBlog.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPostRepository _postRepository;

        public HomeController(IUserRepository userRepository, IPostRepository postRepository) : base(userRepository)
        {
            _postRepository = postRepository;
        }

        public ActionResult Index()
        {
            HomePageViewModel model = new HomePageViewModel();

            var users = UserRepository.GetUsersWithTheirBlogs();

            foreach (var user in users)
            {
                foreach (var blog in user.Blogs)
                {
                    UserBlogViewModel viewModel = new UserBlogViewModel{Name = user.Name, Title = blog.Title, Nickname = blog.Nickname};
                    model.UserBlogViewModels.Add(viewModel);
                }
            }
            var posts = _postRepository.GetPosts();

            foreach (var post in posts)
            {
                const int maxEntryLength = 200;
                string blogPost;
                if (post.BlogPost.Length > maxEntryLength)
                {
                    blogPost = post.BlogPost.Substring(0, maxEntryLength - 4) + " ...";
                }
                else
                {
                    blogPost = post.BlogPost;
                }
                HomePagePostViewModel vm = new HomePagePostViewModel { Title = post.Title, DatePosted = post.Posted, Post = blogPost, UserName = post.Blog.User.Name};
                model.HomePagePostViewModels.Add(vm);
            }

            return View(model);
        }

    }
}
