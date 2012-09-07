using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Logging;
using MBlog.Filters;
using MBlog.Models.Media;
using MBlog.Models.User;
using MBlogModel;
using MBlogServiceInterfaces;

namespace MBlog.Controllers
{
    public partial class MediaController : BaseController
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService, ILogger logger)
            : base(logger)
        {
            _mediaService = mediaService;
        }

        [HttpGet]
        [AuthorizeLoggedInUser]
        public virtual ActionResult Index(string nickname)
        {
            var user = HttpContext.User as UserViewModel;
            IEnumerable<Media> media = _mediaService.GetMedia(1, 10, user.Id);
            var mediaVM = new List<ShowMediaViewModel>();

            foreach (Media medium in media)
            {
                var model = new ShowMediaViewModel(medium);
                model.FileName = medium.FileName;
                model.Id = medium.Id;
                model.Author = nickname;
                mediaVM.Add(model);
            }
            return View(mediaVM);
        }

        [HttpGet]
        public virtual ActionResult Show(int year, int month, int day, string linkkey)
        {
            try
            {
                Media img = _mediaService.GetMedia(year, month, day, linkkey);
                return File(img.Data, img.MimeType);
            }
            catch (MBlogMediaNotFoundException)
            {
                return new HttpNotFoundResult("Unable to load requested media");
            }
        }

        [HttpGet]
        [AuthorizeLoggedInUser]
        public virtual ActionResult New(NewMediaViewModel model)
        {
            return View(new NewMediaViewModel {Nickname = model.Nickname, File = model.File});
        }

        [HttpPost]
        [AuthorizeLoggedInUser]
        public virtual JsonResult Create(NewMediaViewModel model)
        {
            var result = new MediaCreateJsonResponse {success = false};
            try
            {
                var user = (UserViewModel) HttpContext.User;
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
                    var media = new Media();
                    var success = true;
                    var message = "Media uploaded successfully";
                    try
                    {
                        media = _mediaService.WriteMedia(fileName, user.Id, model.ContentType, inputStream, contentLength);
                    }
                    catch (MBlogInsertItemException e)
                    {
                        success = false;
                        message = e.Message;
                    }
                    string anchor = string.Format("<a href='{0}'><img src='{0}'/></a>",
                        Url.Action("show", "media", new{year = media.Year, month = media.Month, day = media.Day, linkKey = media.LinkKey}));
                    result = new MediaCreateJsonResponse {success = success, url = media.Url, message = message, title = media.Title, action = Url.Action("update", "media", new{id = media.Id}), imageAnchor=anchor};
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
        public virtual ActionResult Update(UpdateMediaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new MediaCreateJsonResponse{ success = false, message = "Invalid values" });
            }
            var user = (UserViewModel) HttpContext.User;

            var media = _mediaService.UpdateMediaDetails(model.Id, model.Title, model.Caption, model.Description,
                                                           model.Alternate, user.Id);
            string anchor = string.Format("<a href='{0}'><img src='{0}' class='{1}'/></a>",
                Url.Action("show", "media", new { year = media.Year, month = media.Month, day = media.Day, linkKey = media.LinkKey }),
                model.ClassString);
            return Json(new MediaCreateJsonResponse { success = true, imageAnchor = anchor});
        }

        [HttpGet]
        [AuthorizeLoggedInUser]
        public virtual ActionResult Edit(int mediaId)
        {
            var user = (UserViewModel) HttpContext.User;
            Media media = _mediaService.GetMedia(mediaId, user.Id);
            return View(new ShowMediaViewModel(media));
        }

        [HttpGet]
        [AuthorizeLoggedInUser]
        public virtual ActionResult Delete(int mediaid)
        {
            var user = (UserViewModel)HttpContext.User;
            _mediaService.DeleteMedia(mediaid, user.Id);
            return RedirectToAction("Index", "Media");
        }
    }
}