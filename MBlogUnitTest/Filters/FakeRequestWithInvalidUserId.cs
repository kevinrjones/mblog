using System;
using System.Web;
using MBlog.Infrastructure;

namespace MBlogUnitTest.Filters
{
    class FakeRequestWithInvalidUserId : HttpRequestBase
    {
        public override HttpCookieCollection Cookies
        {
            get
            {
                HttpCookieCollection collection = new HttpCookieCollection();
                byte[] cipherText = "3".Encrypt();
                string cookie = Convert.ToBase64String(cipherText);
                collection.Add(new HttpCookie("USER", cookie));
                return collection;
            }
        }
    }
}