using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Logging;
using MBlog.Infrastructure;
using MBlogNlogService;
using MBlogRepository.Interfaces;
using MBlogRepository.Repositories;
using Microsoft.Practices.Unity;
using Rejuicer;

namespace MBlog
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Error",
                "Error",
                new { controller = "Error", action = "Index" }
            );

            routes.MapRoute(
                "Blog-new",
                "blog/new",
                new { controller = "Blog", action = "New" },
                new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                "Blog-create",
                "blog/create",
                new { controller = "Blog", action = "Create" },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                "Blog-update",
                "blog/update/{blogId}",
                new { controller = "Blog", action = "Update", nickname = UrlParameter.Optional, blogId = UrlParameter.Optional, },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                "Blog-edit",
                "{nickname}/blog/edit/{blogId}",
                new { controller = "Blog", action = "Edit", nickname = UrlParameter.Optional, blogId = UrlParameter.Optional, },
                new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                "Users",
                "user/{action}",
                new { controller = "User", action = "Index" }
            );

            routes.MapRoute(
                "Feed",
                "{nickname}/feed/{action}",
                new { controller = "Feed", action = "rss" },
                new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                "Admin-index",
                "admin/index",
                new { controller = "Admin", action = "Index" }
                );

            routes.MapRoute(
                "Admin",
                "admin/{action}/{nickname}/{blogId}",
                new { controller = "Admin", action = "Index", nickname = UrlParameter.Optional, blogId = UrlParameter.Optional, }
                );

            routes.MapRoute(
                "comments",
                "{nickname}/comment/{action}",
                new { controller = "Comment", action = "Index" }
                );

            routes.MapRoute(
                "Posts-new",
                "{nickname}/new/{blogId}",
                new { controller = "Post", action = "New" },
                new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                "Posts-create",
                "{nickname}/create",
                new { controller = "Post", action = "Create" },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                "Posts-update",
                "{nickname}/update",
                new { controller = "Post", action = "Update" },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                "Posts-edit",
                "{nickname}/edit/{blogId}/{postId}",
                new { controller = "Post", action = "Edit" },
                new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                "Posts-index",
                "{nickname}",
                new { controller = "Post", action = "Index" }
                );

            routes.MapRoute(
                "Posts-show",
                "{nickname}/{year}/{month}/{day}/{link}",
                new { controller = "Post", action = "Show", year = UrlParameter.Optional, month = UrlParameter.Optional, day = UrlParameter.Optional, link = UrlParameter.Optional }
                //,new { year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
                "Default-Home",
                "",
                new { controller = "Home", action = "Index" }
            );


        }

        //protected void Application_Error()
        //{
        //    var error = Server.GetLastError();
        //    // todo: log
        //}

        protected void Application_Start()
        {
            IUnityContainer container = GetUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            AreaRegistration.RegisterAllAreas();

            ConfigureRejuicer(); 

            AddUnityFilterProvider(container);

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

        private static void AddUnityFilterProvider(IUnityContainer container)
        {
            var oldProvider = FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider);
            FilterProviders.Providers.Remove(oldProvider);

            var provider = new UnityFilterAttributeFilterProvider(container);
            FilterProviders.Providers.Add(provider);
        }

        private static void ConfigureRejuicer()
        {            
            //OnRequest.ForJs("~/Combined-{0}.js").Combine.FilesIn("~/Scripts/").Matching("*.js").Configure();
            OnRequest.ForJs("~/Combined-{0}.js").Combine
                .File("~/Scripts/jquery-1.6.2.js")
                .File("~/Scripts/jquery.validate.js")
                .File("~/Scripts/jquery.validate.unobtrusive.js")
                .File("~/Scripts/jquery.unobtrusive-ajax.js")
                .File("~/Scripts/shCore.js")
                .File("~/Scripts/shBrushBash.js")
                .File("~/Scripts/shBrushCss.js")
                .File("~/Scripts/shBrushCSharp.js")
                .File("~/Scripts/shBrushJScript.js")
                .File("~/Scripts/shBrushPowerShell.js")
                .File("~/Scripts/shBrushRuby.js")
                .File("~/Scripts/shBrushSql.js")
                .File("~/Scripts/shBrushXml.js")
                .File("~/Scripts/modernizr-1.7.min.js")
                .Configure();
            OnRequest.ForCss("~/Combined-{0}.css").Compact
                .File("~/Content/Site.css")
                .File("~/Content/shCore.css")
                .File("~/Content/shThemeRDark.css")
                .Configure();
        }

        private IUnityContainer GetUnityContainer()
        {
            InjectionConstructor ctor = new InjectionConstructor(ConfigurationManager.ConnectionStrings["mblog"].ConnectionString);
            IUnityContainer container = new UnityContainer()
                .RegisterType<IUserRepository, UserRepository>(ctor)
                .RegisterType<IPostRepository, PostRepository>(ctor)
                .RegisterType<IBlogRepository, BlogRepository>(ctor)
                .RegisterType<IUsernameBlacklistRepository, UsernameBlacklistRepository>(ctor)
                .RegisterType<INicknameBlacklistRepository, NicknameBlacklistRepository>(ctor)
                .RegisterType<ILogger, NLogService>()
                ;

            return container;
        }
    }
}