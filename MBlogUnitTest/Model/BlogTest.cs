using System;
using MBlogModel;
using NUnit.Framework;

namespace MBlogUnitTest.Model
{
    [TestFixture]
    public class BlogTest
    {
        [Test]
        public void GivenANewBlog_ThenItsLastUpdatedDateIsNow()
        {
            var blog = new Blog();


            Assert.That(blog.LastUpdated, Is.LessThanOrEqualTo(DateTime.Now));
        }
    }
}