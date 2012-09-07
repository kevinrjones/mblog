using Rejuicer;

[assembly: WebActivator.PostApplicationStartMethod(typeof($rootnamespace$.App_Start.RejuicerContent), "Configure")]
namespace $rootnamespace$.App_Start
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
                .Configure();

            OnRequest.ForCss("~/Combined.css")
                .Compact
                .File("~/Content/Site.css")
                .Configure();
            */
        }
    }
}
