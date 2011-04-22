﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MBlog.Controllers;
using MBlog.Models;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    [TestFixture]
    class AdminControllerTest : BaseControllerTests
    {
        [Test]
        public void GivenNoUserInContext_WhenIGoToTheAdminIndexPage_ThenIGetRedirectedToTheHomePage()
        {
            AdminController controller = new AdminController(null);

            SetControllerContext(controller);

            RedirectToRouteResult result = controller.Index() as RedirectToRouteResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Home").IgnoreCase);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index").IgnoreCase);
        }

        [Test]
        public void GivenAUserInContext_WhenIGoToTheAdminIndexPage_ThenIGetRedirectedToTheHomePage()
        {
            AdminController controller = new AdminController(null);

            SetControllerContext(controller);

            MockHttpContext.SetupProperty(h => h.User);
            controller.HttpContext.User = new UserViewModel();

            ViewResult result = controller.Index() as ViewResult;
            Assert.That(result, Is.Not.Null);
        }
    }
}
