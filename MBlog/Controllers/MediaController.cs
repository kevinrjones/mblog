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
using MBlogDomainInterfaces;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlog.Controllers
{
    public class MediaController : BaseController
    {
        private IMediaDomain _mediaDomain;

        public MediaController(IMediaDomain mediaDomain, ILogger logger)
            : base(logger)
        {
            _mediaDomain = mediaDomain;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToRoute("Default-Home");
        }

        [HttpGet]
        public FileResult Show(int year, int month, int day, string fileName)
        {
            Media img = _mediaDomain.GetMedia(year, month, day, fileName);
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
                var user = (UserViewModel)HttpContext.User;
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
                    string url = string.Empty;
                    var success = true;
                    string message = "Created successfully";
                    try
                    {
                        url = _mediaDomain.WriteMedia(fileName, user.Id, model.ContentType, inputStream, contentLength);
                    }
                    catch (MBlogInsertItemException e)
                    {
                        success = false;
                        message = e.Message;
                    }
                    result = new MediaCreateJsonResponse { success = success, url = url, message = message };
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

            var file = model.File;
            if (file != null && file.ContentLength > 0)
            {
                var user = (UserViewModel)HttpContext.User;

                _mediaDomain.WriteMedia(file.FileName, model.Title, model.Caption, model.Description,
                    model.Alternate, user.Id, file.ContentType, model.Alignment, model.Size, file.InputStream, file.ContentLength);
                return RedirectToRoute("new");
            }
            throw new MBlogException("Invalid File");
        }
    }
}
