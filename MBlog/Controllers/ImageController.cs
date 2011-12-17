using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logging;
using MBlog.Filters;
using MBlog.Models.Image;
using MBlog.Models.User;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class ImageController : BaseController
    {
        private readonly IImageRepository _imageRepository;

        public ImageController(IImageRepository imageRepository, IBlogRepository blogRepository, IUserRepository userRepository, ILogger logger)
            : base(logger, userRepository, blogRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpGet]
        public FileResult Show(string urlPrefix, string fileName)
        {
            _imageRepository.GetImage(urlPrefix, fileName);
            return null;
        }

        [HttpGet]
        [AuthorizeBlogOwner]
        public ActionResult New(string nickname, int blogId)
        {
            return View(new NewImageViewModel{Nickname = nickname, BlogId = blogId});
        }


        [HttpPost]
        [AuthorizeBlogOwner]
        public ActionResult Create(string nickname, int blogId, string title, string caption, string description, string alternate, string alignment, string size, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                UserViewModel user = (UserViewModel) HttpContext.User;
                byte[] bytes = new byte[file.ContentLength];
                file.InputStream.Read(bytes, 0, file.ContentLength);
                                
                _imageRepository.WriteImage(file.FileName, title, caption, description, alternate, "", user.Id, file.ContentType, alignment, size, bytes);
                return RedirectToRoute("new");
            }
            throw new MBlogException("Invalid File");
        }

    }
}
