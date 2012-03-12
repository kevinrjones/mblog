using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using IoC;
using Logging;
using MBlog.Controllers;
using MBlog.Infrastructure;
using MBlogDomain;
using MBlogDomainInterfaces;
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
                "Blog-edit",
                "{nickname}/blog/edit/{blogId}",
                new { controller = "Blog", action = "Edit", },
                new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                "Blog-update",
                "{nickname}/blog/update",
                new { controller = "Blog", action = "Update", },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                "Blog-delete",
                "{nickname}/blog/delete/{blogId}",
                new { controller = "Blog", action = "Delete" },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                "Users-new",
                "user/new",
                new { controller = "User", action = "New" },
                new { httpMethod = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                "Users-create",
                "user/create",
                new { controller = "User", action = "Create" },
                new { httpMethod = new HttpMethodConstraint("POST") }
            );

            routes.MapRoute(
                "Session-new",
                "session/new",
                new { controller = "Session", action = "New" },
                new { httpMethod = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                "Session-create",
                "session/create",
                new { controller = "Session", action = "Create" },
                new { httpMethod = new HttpMethodConstraint("POST") }
            );

            routes.MapRoute(
                "Session-delete",
                "session/delete",
                new { controller = "Session", action = "Delete" }
            );

            routes.MapRoute(
                "Feed",
                "{nickname}/feed/{action}",
                new { controller = "Feed", action = "rss" },
                new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                "Dashboard-index",
                "dashboard",
                new { controller = "Dashboard", action = "Index" }
                );

            routes.MapRoute(
                "Admin-posts",
                "admin/posts/{action}/{nickname}/{blogId}",
                new { controller = "Posts", action = "Index", nickname = UrlParameter.Optional, blogId = UrlParameter.Optional, }
            );

            routes.MapRoute(
                "Admin-comments",
                "admin/comments/{action}/{nickname}/{postId}/{blogId}",
                new { controller = "Comments", action = "Index", nickname = UrlParameter.Optional, blogId = UrlParameter.Optional, postId = UrlParameter.Optional, }
            );

            routes.MapRoute(
                "comments",
                "{nickname}/comment/{action}",
                new { controller = "Comment", action = "Index" }
                );

            routes.MapRoute(
                "Media-show",
                "{nickname}/media/{year}/{month}/{day}/{linkkey}",
                new { controller = "media", action = "Show"}
                );

            routes.MapRoute(
                "Media-index",
                "{nickname}/media",
                new { controller = "media", action = "Index" }
                );

            routes.MapRoute(
                "Media-create",
                "{nickname}/media/create",
                new { controller = "Media", action = "Create" },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                "Media-update",
                "{nickname}/media/update/{mediaId}",
                new { controller = "Media", action = "Update", },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                "Media-new",
                "{nickname}/media/new",
                new { controller = "Media", action = "New" },
                new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                "Media-upload",
                "{nickname}/media/upload",
                new { controller = "Media", action = "Upload" },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                "Media-save",
                "{nickname}/media/save",
                new { controller = "Media", action = "Save" },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                "Media-edit",
                "{nickname}/media/edit/{mediaId}",
                new { controller = "Media", action = "Edit", },
                new { httpMethod = new HttpMethodConstraint("GET") }
                );

            //routes.MapRoute(
            //    "Media-delete",
            //    "{nickname}/media/delete/{blogId}/{imageId}",
            //    new { controller = "Media", action = "Delete" },
            //    new { httpMethod = new HttpMethodConstraint("POST") }
            //    );
          
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
            OnRequest.ForJs("~/Combined-{0}.js").Combine
                .File("~/Scripts/jquery-1.7.1.js")
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
                .File("~/Scripts/fileuploader.js")
                .File("~/Scripts/mblog.js")
                .Configure();
            OnRequest.ForCss("~/Combined-{0}.css").Compact
                .File("~/Content/shCore.css")
                .File("~/Content/shThemeRDark.css")
                .File("~/Content/fileuploader.css")
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
                .RegisterType<IMediaRepository, MediaRepository>(ctor)
                .RegisterType<IUserDomain, UserDomain>()
                .RegisterType<IBlogDomain, BlogDomain>()
                .RegisterType<IDashboardDomain, DashboardDomain>()
                .RegisterType<IMediaDomain, MediaDomain>()
                .RegisterType<IPostDomain, PostDomain>()
                .RegisterType<ISyndicationFeedDomain, SyndicationFeedDomain>()
                .RegisterType<ILogger, NLogService>()
                ;

            return container;
        }

    }
}