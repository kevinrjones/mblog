using System;
using System.Web;
using MBlog.Infrastructure;

namespace MBlogUnitTest.Filters
{
    internal class FakeRequestWithValidUserIdButNoBlogs : HttpRequestBase
    {
        public override HttpCookieCollection Cookies
        {
            get
            {
                var collection = new HttpCookieCollection();
                byte[] cipherText = "2".Encrypt();
                string cookie = Convert.ToBase64String(cipherText);
                collection.Add(new HttpCookie("USER", cookie));
                return collection;
            }
        }
    }
}