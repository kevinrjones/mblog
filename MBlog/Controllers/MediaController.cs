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
    public class MediaController : BaseController
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService, ILogger logger)
            : base(logger)
        {
            _mediaService = mediaService;
        }

        [HttpGet]
        [AuthorizeLoggedInUser]
        public ActionResult Index(string nickname)
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
        public ActionResult Show(int year, int month, int day, string linkkey)
        {
            try
            {
                Media img = _mediaService.GetMedia(year, month, day, linkkey);
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
            return View(new NewMediaViewModel {Nickname = model.Nickname, File = model.File});
        }

        [HttpPost]
        [AuthorizeLoggedInUser]
        public JsonResult Create(NewMediaViewModel model)
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
                    var message = "Created successfully";
                    try
                    {
                        media = _mediaService.WriteMedia(fileName, user.Id, model.ContentType, inputStream, contentLength);
                    }
                    catch (MBlogInsertItemException e)
                    {
                        success = false;
                        message = e.Message;
                    }
                    result = new MediaCreateJsonResponse {success = success, url = media.Url, message = message, title = media.Title, action = Url.Action("update", "media", new{mediaId = media.Id})};
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
        public ActionResult Update(UpdateMediaViewModel model)
        {
            // todo: content type?
            if (!ModelState.IsValid)
                return View("edit", model);

            var user = (UserViewModel) HttpContext.User;

            Media media = _mediaService.UpdateMediaDetails(model.Id, model.Title, model.Caption, model.Description,
                                                           model.Alternate, user.Id);

            return View("Edit", new ShowMediaViewModel(media));
        }

        [HttpGet]
        [AuthorizeLoggedInUser]
        public ActionResult Edit(string nickname, int mediaId)
        {
            var user = (UserViewModel) HttpContext.User;
            Media media = _mediaService.GetMedia(mediaId, user.Id);
            return View(new ShowMediaViewModel(media));
        }
    }
}