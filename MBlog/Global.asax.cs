using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Logging;
using MBlog.App_Start;
using MBlogNlogService;
using Rejuicer;

namespace MBlog
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            RegisterTypes(builder);
            builder.RegisterFilterProvider();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            
            AreaRegistration.RegisterAllAreas();

            ConfigureRejuicer();

            GlobalFilters.Filters.RegisterGlobalFilters();
            RouteTable.Routes.RegisterRoutes();
        }

        private static void ConfigureRejuicer()
        {
            OnRequest.ForJs("~/Combined-{0}.js").Combine
                .File("~/Scripts/jquery-1.7.2.js")
                .File("~/Scripts/jquery-ui-1.8.19")
                .File("~/Scripts/jquery.mobile-1.1.0")
                .File("~/Scripts/jquery.unobtrusive-ajax.js")
                .File("~/Scripts/jquery.validate.js")
                .File("~/Scripts/jquery.validate.unobtrusive.js")
                .File("~/Scripts/SyntaxHighlighter/shCore.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushBash.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushCss.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushCSharp.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushJScript.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushPowerShell.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushRuby.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushSql.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushXml.js")
                .File("~/Scripts/modernizr-2.5.3.js")
                .File("~/Scripts/FileUploader/fileuploader.js")
                .File("~/Scripts/mblog/mblog.js")
                .Configure();
            OnRequest.ForCss("~/Combined-{0}.css").Compact
                .File("~/Content/Styles/reset.css")
                .File("~/Content/SyntaxHighlighter/shCore.css")
                .File("~/Content/SyntaxHighlighter/shThemeRDark.css")
                .File("~/Content/fileuploader/fileuploader.css")
                .Configure();
        }

        private void RegisterTypes(ContainerBuilder builder)
        {
            string ctor = ConfigurationManager.ConnectionStrings["mblog"].ConnectionString;
            var repositoryAssemblies = Assembly.Load("MBlogRepository");
            builder.RegisterAssemblyTypes(repositoryAssemblies).AsImplementedInterfaces().WithParameter(new NamedParameter("connectionString", ctor)).InstancePerHttpRequest(); 
            var serviceAssemblies = Assembly.Load("MBlogService");
            builder.RegisterAssemblyTypes(serviceAssemblies).AsImplementedInterfaces();

            builder.RegisterType<NLogService>().As<ILogger>();
        }
    }
}