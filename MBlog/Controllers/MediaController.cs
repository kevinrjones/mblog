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
        private readonly IMediaDomain _mediaDomain;

        public MediaController(IMediaDomain mediaDomain, ILogger logger)
            : base(logger)
        {
            _mediaDomain = mediaDomain;
        }

        [HttpGet]
        [AuthorizeLoggedInUser]
        public ActionResult Index(string nickname)
        {
            var user = HttpContext.User as UserViewModel;
            var media = _mediaDomain.GetMedia(1, 10, user.Id);
            var mediaVM = new List<ShowMediaViewModel>();

            foreach (var medium in media)
            {
                ShowMediaViewModel model = new ShowMediaViewModel(medium);
                model.FileName = medium.FileName;
                model.Id = medium.Id;
                model.Author = nickname;
                mediaVM.Add(model);
            }
            return View(mediaVM);
        }

        [HttpGet]
        public ActionResult Show(int year, int month, int day, string title)
        {
            try
            {
                Media img = _mediaDomain.GetMedia(year, month, day, title);
                return new FileContentResult(img.Data, img.MimeType);
            }
            catch (MBlogMediaNotFoundException)
            {
                return new HttpNotFoundResult("Unable to load requested media");
            }
        }

        [HttpGet]
        [AuthorizeLoggedInUser]
        public ActionResult New(NewMediaViewModel model)
        {
            return View(new NewMediaViewModel { Nickname = model.Nickname, File = model.File });
        }

        [HttpPost]
        [AuthorizeLoggedInUser]
        public JsonResult Create(NewMediaViewModel model)
        {
            var result = new MediaCreateJsonResponse { success = false };
            try
            {
                var user = (UserViewModel)HttpContext.User;
                int contentLength;
                Stream inputStream;
                string fileName;
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
        [AuthorizeLoggedInUser]
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

        [HttpGet]
        [AuthorizeLoggedInUser]
        public ActionResult Edit(string nickname, int mediaId)
        {
            var user = (UserViewModel)HttpContext.User;
            var media = _mediaDomain.GetMedia(mediaId, user.Id);
            return View(new ShowMediaViewModel(media));
        }


    }
}
