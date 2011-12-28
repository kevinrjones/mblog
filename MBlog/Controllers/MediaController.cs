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
    public class MediaController : BaseController
    {
        private readonly IMediaRepository _mediaRepository;

        public MediaController(IMediaRepository mediaRepository, IBlogRepository blogRepository, IUserRepository userRepository, ILogger logger)
            : base(logger, userRepository, blogRepository)
        {
            _mediaRepository = mediaRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {            
            return RedirectToRoute("Default-Home");
        }

        [HttpGet]
        public FileResult Show(int year, int month, int day, string fileName)
        {
            Media img = _mediaRepository.GetMedia(year, month, day, fileName);
            return new FileContentResult(img.ImageData, img.MimeType);
        }

        [HttpGet]
        [AuthorizeBlogOwner]
        public ActionResult New(string nickname, int blogId)
        {
            return View(new NewMediaViewModel{Nickname = nickname, BlogId = blogId});
        }


        [HttpPost]
        [AuthorizeBlogOwner]
        public ActionResult Create(string title, string caption, string description, string alternate, int alignment, int size, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                UserViewModel user = (UserViewModel) HttpContext.User;
                byte[] bytes = new byte[file.ContentLength];
                file.InputStream.Read(bytes, 0, file.ContentLength);

                // todo: url?                
                Media img = new Media (file.FileName, title, caption,  description, 
                    alternate, user.Id, file.ContentType, alignment, size, bytes);
                _mediaRepository.WriteMedia(img);
                return RedirectToRoute("new");
            }
            throw new MBlogException("Invalid File");
        }

    }
}
