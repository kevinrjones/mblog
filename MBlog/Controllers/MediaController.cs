using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logging;
using MBlog.Filters;
using MBlog.Models.Media;
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
            return new FileContentResult(img.MediumData, img.MimeType);
        }

        [HttpGet]
        [AuthorizeBlogOwner]
        public ActionResult New(NewMediaViewModel model)
        {
            return View(new NewMediaViewModel { Nickname = model.Nickname, BlogId = model.BlogId, File = model.File });
        }

        [HttpPost]
        [AuthorizeBlogOwner]
        public ActionResult Upload(NewMediaViewModel model)
        {
            if (!ModelState.IsValid)
                return View("new", model);

            HttpPostedFileBase file = model.File;
            if (file != null && file.ContentLength > 0)
            {
                UserViewModel user = (UserViewModel)HttpContext.User;
                byte[] bytes = new byte[file.ContentLength];
                file.InputStream.Read(bytes, 0, file.ContentLength);

                // todo: url?                
                Media img = new Media(file.FileName, user.Id, file.ContentType, bytes);
                _mediaRepository.WriteMedia(img);
                return RedirectToAction("new");
            }

            return RedirectToAction("new");
        }

        [HttpPost]
        [AuthorizeBlogOwner]
        public ActionResult Create(string title, string caption, string description, string alternate, int alignment, int size, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                UserViewModel user = (UserViewModel)HttpContext.User;
                byte[] bytes = new byte[file.ContentLength];
                file.InputStream.Read(bytes, 0, file.ContentLength);

                // todo: url?                
                Media img = new Media(file.FileName, title, caption, description,
                    alternate, user.Id, file.ContentType, alignment, size, bytes);
                _mediaRepository.WriteMedia(img);
                return RedirectToRoute("new");
            }
            throw new MBlogException("Invalid File");
        }

    }
}
