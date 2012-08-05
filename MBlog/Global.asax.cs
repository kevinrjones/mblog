using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Logging;
using MBlog.Infrastructure;
using MBlogNlogService;
using MBlogRepository.Interfaces;
using MBlogRepository.Repositories;
using MBlogService;
using MBlogServiceInterfaces;
using Rejuicer;

namespace MBlog
{
    public class MvcApplication : HttpApplication
    {
        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Error",
                url: "Error",
                defaults: new { controller = "Error", action = "Index" }
                );

            routes.MapRoute(
                name: "Blog-new",
                url: "blog/new",
                defaults: new { controller = "Blog", action = "New" },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                name: "Blog-create",
                url: "blog/create",
                defaults: new { controller = "Blog", action = "Create" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                name: "Blog-edit",
                url: "{nickname}/blog/edit/{blogId}",
                defaults: new { controller = "Blog", action = "Edit", },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                name: "Blog-update",
                url: "{nickname}/blog/update",
                defaults: new { controller = "Blog", action = "Update", },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                name: "Blog-delete",
                url: "{nickname}/blog/delete/{blogId}",
                defaults: new { controller = "Blog", action = "Delete" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                name: "Users-new",
                url: "user/new",
                defaults: new { controller = "User", action = "New" },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                name: "Users-create",
                url: "user/create",
                defaults: new { controller = "User", action = "Create" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                name: "Session-new",
                url: "session/new",
                defaults: new { controller = "Session", action = "New" },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                name: "Session-create",
                url: "session/create",
                defaults: new { controller = "Session", action = "Create" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                name: "Session-delete",
                url: "session/delete",
                defaults: new { controller = "Session", action = "Delete" }
                );

            routes.MapRoute(
                name: "Feed",
                url: "{nickname}/feed/{action}",
                defaults: new { controller = "Feed", action = "rss" },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                name: "Dashboard-index",
                url: "dashboard",
                defaults: new { controller = "Dashboard", action = "Index" }
                );

            routes.MapRoute(
                name: "Admin-posts",
                url: "admin/posts/{action}/{nickname}/{blogId}",
                defaults:
                    new
                        {
                            controller = "Posts",
                            action = "Index",
                            nickname = UrlParameter.Optional,
                            blogId = UrlParameter.Optional,
                        }
                );

            routes.MapRoute(
                name: "Admin-comments",
                url: "admin/comments/{action}/{nickname}/{postId}/{blogId}",
                defaults:
                    new
                        {
                            controller = "Comments",
                            action = "Index",
                            nickname = UrlParameter.Optional,
                            blogId = UrlParameter.Optional,
                            postId = UrlParameter.Optional,
                        }
                );

            routes.MapRoute(
                name: "comments",
                url: "{nickname}/comment/{action}",
                defaults: new { controller = "Comment", action = "Index" }
                );

            routes.MapRoute(
                name: "Media-show",
                url: "{nickname}/media/{year}/{month}/{day}/{linkkey}",
                defaults: new { controller = "media", action = "Show" }
                );

            routes.MapRoute(
                name: "Media-index",
                url: "{nickname}/media",
                defaults: new { controller = "media", action = "Index" }
                );

            routes.MapRoute(
                name: "Media-create",
                url: "{nickname}/media/create",
                defaults: new { controller = "Media", action = "Create" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                name: "Media-update",
                url: "{nickname}/media/update/{id}",
                defaults: new { controller = "Media", action = "Update", },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                name: "Media-new",
                url: "{nickname}/media/new",
                defaults: new { controller = "Media", action = "New" },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                name: "Media-upload",
                url: "{nickname}/media/upload",
                defaults: new { controller = "Media", action = "Upload" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                name: "Media-save",
                url: "{nickname}/media/save",
                defaults: new { controller = "Media", action = "Save" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                name: "Media-edit",
                url: "{nickname}/media/edit/{mediaId}",
                defaults: new { controller = "Media", action = "Edit", },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                name: "Media-delete",
                url: "{nickname}/media/delete/{id}",
                defaults: new { controller = "Media", action = "Delete", },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                name: "Posts-new",
                url: "{nickname}/new/{blogId}",
                defaults: new { controller = "Post", action = "New" },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                name: "Posts-create",
                url: "{nickname}/create",
                defaults: new { controller = "Post", action = "Create" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                name: "Posts-update",
                url: "{nickname}/update",
                defaults: new { controller = "Post", action = "Update" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );

            routes.MapRoute(
                name: "Posts-edit",
                url: "{nickname}/edit/{blogId}/{postId}",
                defaults: new { controller = "Post", action = "Edit" },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );

            routes.MapRoute(
                name: "Posts-index",
                url: "{nickname}",
                defaults: new { controller = "Post", action = "Index" }
                );

            routes.MapRoute(
                name: "Posts-show",
                url: "{nickname}/{year}/{month}/{day}/{link}",
                defaults:
                    new
                        {
                            controller = "Post",
                            action = "Show",
                            year = UrlParameter.Optional,
                            month = UrlParameter.Optional,
                            day = UrlParameter.Optional,
                            link = UrlParameter.Optional
                        }
                //,new { year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
                name: "Default-Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
                );
        }

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

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private static void ConfigureRejuicer()
        {
            OnRequest.ForJs("~/Combined-{0}.js").Combine
                .File("~/Scripts/jquery-1.7.1.js")
                .File("~/Scripts/jquery.validate.js")
                .File("~/Scripts/jquery.validate.unobtrusive.js")
                .File("~/Scripts/jquery.unobtrusive-ajax.js")
                .File("~/Scripts/SyntaxHighlighter/shCore.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushBash.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushCss.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushCSharp.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushJScript.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushPowerShell.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushRuby.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushSql.js")
                .File("~/Scripts/SyntaxHighlighter/shBrushXml.js")
                .File("~/Scripts/modernizr-2.0.6-development-only.js")
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
            builder.RegisterType<UserRepository>().As<IUserRepository>().WithParameter(new NamedParameter("connectionString", ctor)).InstancePerHttpRequest();
            builder.RegisterType<PostRepository>().As<IPostRepository>().WithParameter(new NamedParameter("connectionString", ctor)).InstancePerHttpRequest();
            builder.RegisterType<BlogRepository>().As<IBlogRepository>().WithParameter(new NamedParameter("connectionString", ctor)).InstancePerHttpRequest();
            builder.RegisterType<UsernameBlacklistRepository>().As<IUsernameBlacklistRepository>().WithParameter(new NamedParameter("connectionString", ctor)).InstancePerHttpRequest();
            builder.RegisterType<NicknameBlacklistRepository>().As<INicknameBlacklistRepository>().WithParameter(new NamedParameter("connectionString", ctor)).InstancePerHttpRequest();
            builder.RegisterType<MediaRepository>().As<IMediaRepository>().WithParameter(new NamedParameter("connectionString", ctor)).InstancePerHttpRequest();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerHttpRequest();
            builder.RegisterType<BlogService>().As<IBlogService>().InstancePerHttpRequest();
            builder.RegisterType<DashboardService>().As<IDashboardService>().InstancePerHttpRequest();
            builder.RegisterType<MediaService>().As<IMediaService>().InstancePerHttpRequest();
            builder.RegisterType<PostService>().As<IPostService>().InstancePerHttpRequest();
            builder.RegisterType<SyndicationFeedService>().As<ISyndicationFeedService>().InstancePerHttpRequest();
            builder.RegisterType<NLogService>().As<ILogger>().InstancePerHttpRequest();
        }
    }
}