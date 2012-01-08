using System;
using System.Collections.Generic;
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
            : base(logger, null, null)
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
                var user = (UserViewModel)HttpContext.User;

                _mediaDomain.WriteMedia(file.FileName, user.Id, file.ContentType, file.InputStream, file.ContentLength);
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
                var user = (UserViewModel)HttpContext.User;

                _mediaDomain.WriteMedia(file.FileName, title, caption, description,
                    alternate, user.Id, file.ContentType, alignment, size, file.InputStream, file.ContentLength);
                return RedirectToRoute("new");
            }
            throw new MBlogException("Invalid File");
        }

    }
}
