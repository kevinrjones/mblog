using System;
using System.Collections.Generic;
using System.IO;
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
            return new FileContentResult(img.Data, img.MimeType);
        }

        [HttpGet]
        [AuthorizeBlogOwner]
        public ActionResult New(NewMediaViewModel model)
        {
            return View(new NewMediaViewModel { Nickname = model.Nickname, BlogId = model.BlogId, File = model.File });
        }

        [HttpPost]
        [AuthorizeBlogOwner]
        public JsonResult Create(NewMediaViewModel model)
        {
            int contentLength;
            Stream inputStream;
            string fileName;

            var result = new MediaCreateJsonResponse { success = false };
            try
            {
                if (model.File != null)
                {
                    contentLength = model.File.ContentLength;
                    inputStream = model.File.InputStream;
                    fileName = model.File.FileName;
                }
                else
                {
                    contentLength = HttpContext.Request.ContentLength;
                    inputStream = HttpContext.Request.InputStream;
                    fileName = model.QqFile;
                }

                if (contentLength != 0)
                {
                    var media = new Media(fileName, model.BlogId, model.ContentType, inputStream, contentLength);
                    _mediaRepository.WriteMedia(media);
                    result = new MediaCreateJsonResponse { success = true, url = media.Url };
                }
            }
            catch (Exception e)
            {
                result.exception = e;
                result.message = e.Message;
            }
            return Json(result);
        }

        [HttpPost]
        [AuthorizeBlogOwner]
        public ActionResult Update(NewMediaViewModel model)
        {
            if (!ModelState.IsValid)
                return View("new", model);

            return RedirectToAction("new");
        }
    }
}
