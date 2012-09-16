using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MBlog.Infrastructure
{
    public class UrlGenerator
    {
        public static string GenerateUrl(string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, RouteCollection routeCollection, RequestContext requestContext, bool includeImplicitMvcValues)
        {
            string str1 = UrlHelper.GenerateUrl(routeName, actionName, controllerName, routeValues, routeCollection, requestContext, includeImplicitMvcValues);
            if (str1 != null)
            {
                if (!string.IsNullOrEmpty(fragment))
                    str1 = str1 + "#" + fragment;
                if (!string.IsNullOrEmpty(protocol) || !string.IsNullOrEmpty(hostName))
                {
                    Uri url = requestContext.HttpContext.Request.Url;
                    protocol = !string.IsNullOrEmpty(protocol) ? protocol : Uri.UriSchemeHttp;
                    hostName = !string.IsNullOrEmpty(hostName) ? hostName : url.Host;
                    str1 = protocol + Uri.SchemeDelimiter + hostName + str1;
                }
            }
            return str1;
        }
    }
}