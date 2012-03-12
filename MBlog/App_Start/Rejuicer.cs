using Rejuicer;

[assembly: WebActivator.PostApplicationStartMethod(typeof(MBlog.App_Start.RejuicerContent), "Configure")]
namespace MBlog.App_Start
{
    public static class RejuicerContent
    {
        public static void Configure()
        {
            /*
            OnRequest.ForJs("~/Combined-{0}.js")
                .Compact
                .FilesIn("~/Scripts/")
                .Matching("*.js")
                .FilesIn("~/Scripts/")  // Include coffee script, these will be automatically compiled to javascript
                .Matching("*.coffee")
                .Configure();

            OnRequest.ForCss("~/Combined.css")
                .Compact
                .File("~/Content/Site.css")
                .Configure();
            */
        }
    }
}
