using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Blog blog = new Blog();


            Assert.That(blog.LastUpdated, Is.LessThanOrEqualTo(DateTime.Now));
        }
    }
}
