using System;
using System.Web.Mvc;

namespace MBlog.Filters
{
    public class HttpBasicUnauthorizedResult : HttpUnauthorizedResult
    {
        // the base class already assigns the 401.
        // we bring these constructors with us to allow setting status text
        public HttpBasicUnauthorizedResult() : base()
        {
        }

        public HttpBasicUnauthorizedResult(string statusDescription) : base(statusDescription)
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            context.HttpContext.Response.AddHeader("WWW-Authenticate", "Basic");
            base.ExecuteResult(context);
        }
    }
}