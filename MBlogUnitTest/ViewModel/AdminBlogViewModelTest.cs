using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlog.Models.Admin;
using NUnit.Framework;

namespace MBlogUnitTest.ViewModel
{
    [TestFixture]
    public class AdminBlogViewModelTest
    {
        [Test]
        public void GivenATitle_WhenItIsLessThan60CharsLong_ThenIGetBackTheWholeTitle()
        {
            string title = "1234567890";
            AdminBlogViewModel model = new AdminBlogViewModel();
            model.Title = title;
            Assert.That(model.Title.Length, Is.EqualTo(title.Length));
        }

        [Test]
        public void GivenATitle_WhenItIsMoreThan60CharsLong_ThenIGetBackTheFirst57CharsAndThenDots()
        {
            string title = "1234567890123456789012345678901234567890123456789012345678901234567890";
            AdminBlogViewModel model = new AdminBlogViewModel();
            model.Title = title;
            Assert.That(model.Title.Length, Is.EqualTo(60));
            Assert.That(model.Title, Is.StringEnding("..."));
        }

        [Test]
        public void GivenATitle_WhenItIs59CharsLong_ThenIGetBackTheWholeTitle()
        {
            string title = "12345678901234567890123456789012345678901234567890123456789";
            AdminBlogViewModel model = new AdminBlogViewModel();
            model.Title = title;
            Assert.That(model.Title.Length, Is.EqualTo(59));
        }

        [Test]
        public void GivenATitle_WhenItIs60CharsLong_ThenIGetBackTheFirst57CharsAndThenDots()
        {
            string title = "123456789012345678901234567890123456789012345678901234567890";
            AdminBlogViewModel model = new AdminBlogViewModel();
            model.Title = title;
            Assert.That(model.Title.Length, Is.EqualTo(60));
            Assert.That(model.Title, Is.StringEnding("..."));
        }
    }
}
