using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBlog.Models.Admin;
using NUnit.Framework;

namespace MBlogUnitTest.ViewModel
{
    [TestFixture]
    public class AdminUserViewModelTest
    {
        [Test]
        public void GivenANewAdminUserVewModel_WhenIAccessTheInitialBlogCollection_ThenItIsNotNull()
        {
            AdminUserViewModel model = new AdminUserViewModel();
            Assert.That(model.Blogs, Is.Not.Null);
        }

        [Test]
        public void GivenANewBlogCollection_WhenIAccessTheCollection_ThenItIsTheSameAsTheSetValue()
        {
            List<AdminBlogViewModel> blogs = new List<AdminBlogViewModel>();
            AdminUserViewModel model = new AdminUserViewModel();
            model.Blogs = blogs;
            Assert.That(model.Blogs, Is.EqualTo(blogs));
        }
    }
}
