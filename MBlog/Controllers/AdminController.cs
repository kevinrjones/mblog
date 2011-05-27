using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBlog.Models;
using MBlog.Models.Admin;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(IUserRepository userRepository) : base(userRepository)
        {}

        public ActionResult Index()
        {
            UserViewModel user = HttpContext.User as UserViewModel;
            if (user == null || !user.IsLoggedIn)
            {
                return RedirectToAction("login", "user");
            }
            var users =  UserRepository.GetUserWithTheirBlogs(user.Id);
            AdminUserViewModel adminUserViewModel = new AdminUserViewModel{Name = user.Name};
            foreach (Blog blog in users.Blogs)
            {
                adminUserViewModel.Blogs.Add(new AdminBlogViewModel {Name = blog.Nickname, Title = blog.Title, Description = blog.Description});
            }
            return View(adminUserViewModel);
        }
    }
}
