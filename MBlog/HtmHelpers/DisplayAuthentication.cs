using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MBlog.HtmHelpers
{
    public static class DisplayAuthentication
    {
        public static MvcHtmlString AuthenticationStatus(this HtmlHelper html)
        {            
            string outputString;
            if (HttpContext.Current.User == null || !HttpContext.Current.User.Identity.IsAuthenticated)
            {
                outputString = string.Format("<li>{0}</li> <li>{1}</li>", html.ActionLink("register", "New", "user", null, null), html.ActionLink("login", "New", "Session", null, null));
            }
            else
            {
                outputString = string.Format("<li>{0}</li> <li>{1}</li>", html.ActionLink("dashboard", "Index", "Dashboard", null, null), html.ActionLink("logout", "Delete", "session", null, null));
            }
            return new MvcHtmlString(outputString);
        }
    }
}