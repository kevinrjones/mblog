using System.Configuration;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using MBlog.Infrastructure;
using MBlogModel;
using MBlogRepository;
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
                "Show",
                "{controller}/show/{id}",
                new { controller = "Home", action = "show" },
                new { httpMethod = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                "Create",
                "{controller}/create",
                new { controller = "Home", action = "create" },
                new { httpMethod = new HttpMethodConstraint("POST") }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }

        protected void Application_Start()
        {
            IUnityContainer container = GetUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private IUnityContainer GetUnityContainer()
        {
            InjectionConstructor ctor = new InjectionConstructor(ConfigurationManager.ConnectionStrings["mblog"].ConnectionString);
            IUnityContainer container = new UnityContainer()
                .RegisterType<IBlogRepository, BlogRepository>(ctor)
                ;

            return container;
        }
    }
}