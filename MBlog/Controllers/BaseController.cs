using System.Web.Mvc;
using MBlog.Filters;
using MBlogRepository;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    [GetCookieUserFilter]
    public class BaseController : Controller
    {
        internal IUserRepository UserRepository { get; set; }

        public BaseController(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

    }
}