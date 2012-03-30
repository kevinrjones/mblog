using System.Collections.Generic;
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
            var model = new AdminUserViewModel();
            Assert.That(model.Blogs, Is.Not.Null);
        }

        [Test]
        public void GivenANewBlogCollection_WhenIAccessTheCollection_ThenItIsTheSameAsTheSetValue()
        {
            var blogs = new List<AdminBlogViewModel>();
            var model = new AdminUserViewModel();
            model.Blogs = blogs;
            Assert.That(model.Blogs, Is.EqualTo(blogs));
        }
    }
}