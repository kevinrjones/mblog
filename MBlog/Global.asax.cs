using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using MBlog.Infrastructure;
using MBlogRepository;
using MBlogRepository.Interfaces;
using MBlogRepository.Repositories;
using Microsoft.Practices.Unity;

namespace MBlog
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Error",
                "Error",
                new { controller = "Error", action = "Index" }
            );

            routes.MapRoute(
                "Users",
                "user/{action}",
                new { controller = "User", action = "Index" }
            );

            routes.MapRoute(
                "Admin",
                "admin/{action}",
                new { controller = "Admin", action = "Index" }
                );

            routes.MapRoute(
                "Posts-index",
                "{nickname}",
                new { controller = "Post", action = "Index" }
                );

            routes.MapRoute(
                "Posts-others",
                "{nickname}/{year}/{month}/{day}/{link}",
                new { controller = "Post", action = "Show", year = UrlParameter.Optional, month = UrlParameter.Optional, day = UrlParameter.Optional, link = UrlParameter.Optional }
                //,new { year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            
            routes.MapRoute(
                "Default-Home",
                "",
                new { controller = "Home", action = "Index"}
            );


        }

        protected void Application_Start()
        {
            IUnityContainer container = GetUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

        private IUnityContainer GetUnityContainer()
        {
            InjectionConstructor ctor = new InjectionConstructor(ConfigurationManager.ConnectionStrings["mblog"].ConnectionString);
            IUnityContainer container = new UnityContainer()
                .RegisterType<IBlogPostRepository, BlogPostRepository>(ctor)
                .RegisterType<IUserRepository, UserRepository>(ctor)
                .RegisterType<IPostRepository, PostRepository>(ctor)
                .RegisterType<IUsernameBlacklistRepository, UsernameBlacklistRepository>(ctor)
                .RegisterType<INicknameBlacklistRepository, NicknameBlacklistRepository>(ctor)
                ;

            return container;
        }
    }
}